// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;
using System.Collections.Generic;

namespace Fl.TypeChecking.Inferrers
{
    public class CallableTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstCallableNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstCallableNode node)
        {
            // Get the callable inferred type (and symbol)
            var callableInferredType = node.Callable.Visit(visitor);

            // Get the symbol function
            var symbol = callableInferredType.Symbol;

            // If symbol is not a function, throw an exception
            // TODO: This could be replaced by structural typing, using the operator() method
            if (!(symbol is Function function))                
                throw new System.Exception($"Cannot invoke a non-function object");

            // Check parameters count
            // TODO: This is not needed to be here
            if (function.Parameters.Length != node.Arguments.Expressions.Count)
                throw new System.Exception($"Function {function.Name} expects {function.Parameters.Length} arguments but received {node.Arguments.Expressions.Count}");

            // Get the parameters used to invoke the target function
            var parameters = new List<Type>();

            // Iterate over the function parameters and infer types if needed
            for (var i = 0; i < function.Parameters.Length; i++)
            {
                // Get the inferred argument type for this call
                var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                // Get the declared parameter symbol
                var paramSymbol = function.GetSymbol(function.Parameters[i]);

                // If the parameter does not have a type, assume it
                if (paramSymbol.Type == null)
                    visitor.Inferrer.AssumeSymbolType(paramSymbol);

                // If possible, make conclusions about the inferred argument type and the parameter type
                visitor.Inferrer.MakeConclusion(paramSymbol.Type, inferredParamType.Type);

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
