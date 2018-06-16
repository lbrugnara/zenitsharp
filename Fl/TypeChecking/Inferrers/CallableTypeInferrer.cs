// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols.Types;
using Fl.Ast;
using Fl.Symbols.Types;
using System.Collections.Generic;

namespace Fl.TypeChecking.Inferrers
{
    public class CallableTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstCallableNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstCallableNode node)
        {
            // Get the callable inferred type (and symbol)
            var inferredInfo = node.Callable.Visit(visitor);

            if (inferredInfo.Type is Anonymous)
                return this.CallableAnonymous(visitor, node, inferredInfo);

            if (inferredInfo.Type is Function)
                    return this.CallableFunction(visitor, node, inferredInfo);

            throw new System.Exception($"Cannot invoke a non-function object");
        }

        private InferredType CallableAnonymous(TypeInferrerVisitor visitor, AstCallableNode node, InferredType inferred)
        {
            var symbol = inferred.Symbol;

            // Get the parameters used to invoke the target function
            var parameters = new List<Type>();

            Function anonFunc = new Function(visitor.SymbolTable.Global);

            // Iterate over the function parameters and infer types if needed
            for (var i = 0; i < node.Arguments.Count; i++)
            {
                // Get the inferred argument type for this call
                var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                anonFunc.DefineParameter(null, inferredParamType.Type);

                // Save the inferred type
                parameters.Add(inferredParamType.Type);
            }

            // Inferred type at Callable node will be the target's return type
            var retSymbol = anonFunc.GetSymbol("@ret");

            // If the type is not yet infererd, assign a temporal one
            if (retSymbol.Type == null)
                visitor.Inferrer.AssumeSymbolType(retSymbol);

            //visitor.Inferrer.AssumeSymbolTypeAs(symbol, anonFunc);
            visitor.Inferrer.MakeConclusion(anonFunc, symbol.Type);

            // This invocation will have the function's return type
            return new InferredType(retSymbol.Type);
        }

        private InferredType CallableFunction(TypeInferrerVisitor visitor, AstCallableNode node, InferredType inferred)
        {
            var function = inferred.Type as Function;

            // Check parameters count
            // TODO: This is not needed to be here
            if (function.Parameters.Count != node.Arguments.Expressions.Count)
                throw new System.Exception($"Function {function.Name} expects {function.Parameters.Count} arguments but received {node.Arguments.Expressions.Count}");

            // Get the parameters used to invoke the target function
            var parameters = new List<Type>();

            // Iterate over the function parameters and infer types if needed
            for (var i = 0; i < function.Parameters.Count; i++)
            {
                // Get the inferred argument type for this call
                var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                // Get the declared parameter symbol
                var paramSymbol = function.GetSymbol(function.Parameters[i]);

                // If the parameter does not have a type, assume it
                if (paramSymbol.Type == null)
                    visitor.Inferrer.AssumeSymbolType(paramSymbol);

                // If possible, make conclusions about the inferred argument type and the parameter type
                visitor.Inferrer.MakeConclusion(inferredParamType.Type, paramSymbol.Type);

                // Save the inferred type
                parameters.Add(inferredParamType.Type);
            }

            // Inferred type at Callable node will be the target's return type
            var retSymbol = function.GetSymbol("@ret");

            // If the type is not yet infererd, assign a temporal one
            if (retSymbol.Type == null)
                visitor.Inferrer.AssumeSymbolType(retSymbol);

            // This invocation will have the function's return type
            return new InferredType(retSymbol.Type);
        }
    }
}
