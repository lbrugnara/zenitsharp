// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols.Types;
using Fl.Ast;
using Fl.Symbols.Types;
using System.Collections.Generic;
using Fl.Symbols;

namespace Fl.TypeChecking.Inferrers
{
    public class CallableTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstCallableNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstCallableNode node)
        {
            // Get the callable inferred type (and symbol)
            var inferredInfo = node.Callable.Visit(visitor);

            // If the inferred type is an anonymous type, it means the target symbol has not
            // been defined yet, we could infer its type, at least at a certain level
            if (inferredInfo.Type is Anonymous)
                return this.CallableAnonymous(visitor, node, inferredInfo);

            // If the inferred type is a Function type, we have more information about the target,
            // we can potentially infer more types
            if (inferredInfo.Type is Function)
                    return this.CallableFunction(visitor, node, inferredInfo);

            throw new System.Exception($"Cannot invoke a non-function object");
        }

        private InferredType CallableAnonymous(TypeInferrerVisitor visitor, AstCallableNode node, InferredType inferred)
        {
            var symbol = inferred.Symbol;

            // Get the parameters used to invoke the target function
            var parameters = new List<Type>();

            Function anonFunc = new Function();

            // Iterate over the function parameters and infer types if needed
            for (var i = 0; i < node.Arguments.Count; i++)
            {
                // Get the inferred argument type for this call
                var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                anonFunc.DefineParameterType(inferredParamType.Type);

                // Save the inferred type
                parameters.Add(inferredParamType.Type);
            }


            var rettype = visitor.Inferrer.NewAnonymousType();
            anonFunc.SetReturnType(rettype);

            //visitor.Inferrer.AssumeSymbolTypeAs(symbol, anonFunc);
            visitor.Inferrer.MakeConclusion(anonFunc, symbol.Type);

            // This invocation will have the function's return type
            return new InferredType(rettype);
        }

        private InferredType CallableFunction(TypeInferrerVisitor visitor, AstCallableNode node, InferredType inferred)
        {
            var funcType = inferred.Type as Function;

            // Check parameters count
            // TODO: This is not needed to be here
            if (funcType.Parameters.Count != node.Arguments.Expressions.Count)
                throw new System.Exception($"Function {funcType.Name} expects {funcType.Parameters.Count} arguments but received {node.Arguments.Expressions.Count}");

            // Get the parameters used to invoke the target function
            var parameters = new List<Type>();

            /*
            if (inferred.Symbol is FunctionSymbol)
            {
                var funcSymbol = inferred.Symbol as FunctionSymbol;

                // Iterate over the function parameters and infer types if needed
                for (var i = 0; i < funcType.Parameters.Count; i++)
                {
                    // Get the inferred argument type for this call
                    var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                    // Get the declared parameter symbol
                    var paramSymbol = funcSymbol.Parameters[i];

                    // If the parameter does not have a type, assume it
                    if (paramSymbol.Type == null)
                        visitor.Inferrer.AssumeSymbolType(paramSymbol);

                    // If possible, make conclusions about the inferred argument type and the parameter type
                    visitor.Inferrer.MakeConclusion(inferredParamType.Type, paramSymbol.Type);

                    // Save the inferred type
                    parameters.Add(inferredParamType.Type);
                }

                // Inferred type at Callable node will be the target's return type
                // If the type is not yet infererd, assign a temporal one
                if (funcSymbol.Return.Type == null)
                    visitor.Inferrer.AssumeSymbolType(funcSymbol.Return);

                // This invocation will have the function's return type
                return new InferredType(funcSymbol.Return.Type);
            }*/

            // Iterate over the function parameters and infer types if needed
            for (var i = 0; i < funcType.Parameters.Count; i++)
            {
                // Get the inferred argument type for this call
                var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                // If possible, make conclusions about the inferred argument type and the parameter type
                visitor.Inferrer.MakeConclusion(inferredParamType.Type, funcType.Parameters[i]);

                // Save the inferred type
                parameters.Add(inferredParamType.Type);
            }

            // This invocation will have the function's return type
            return new InferredType(funcType.Return);
        }
    }
}
