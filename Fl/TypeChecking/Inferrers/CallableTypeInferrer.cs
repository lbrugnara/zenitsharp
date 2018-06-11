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

            if (symbol is Function function)
            {
                // Check parameters cound
                // TODO: This is not needed to be here
                if (function.Parameters.Length != node.Arguments.Expressions.Count)
                    throw new System.Exception($"Function {function.Name} expects {function.Parameters.Length} arguments but received {node.Arguments.Expressions.Count}");

                // Get the parameters used to invoke the target function
                var parameters = new List<Type>();

                // Iterate over the function parameters and infer types if needed
                for (var i = 0; i < function.Parameters.Length; i++)
                {
                    // Get parameter symbol
                    var paramSymbol = function.GetSymbol(function.Parameters[i]);

                    // Get the inferred parameter type for this call
                    var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                    // If needed, unify the types
                    if (paramSymbol.Type == null)
                        visitor.Inferrer.AssumeSymbolType(paramSymbol);

                    visitor.Inferrer.UnifyTypesIfPossible(paramSymbol.Type, inferredParamType.Type);

                    // Save the inferred type
                    parameters.Add(inferredParamType.Type);
                }

                // Inferred type at Callable node will be the target's return type
                var retSymbol = function.GetSymbol("@ret");

                // If the type is not yet infererd, assign a temporal one
                if (retSymbol.Type == null)
                    visitor.Inferrer.AssumeSymbolType(retSymbol);

                return new InferredType(retSymbol.Type);
            }
            else if (callableInferredType.Type is Func typeFunc)
            {
                // Check parameters cound
                // TODO: This is not needed to be here
                if (typeFunc.Parameters.Length != node.Arguments.Expressions.Count)
                    throw new System.Exception($"Function {symbol.Name} expects {typeFunc.Parameters.Length} arguments but received {node.Arguments.Expressions.Count}");

                // Get the parameters used to invoke the target function
                var parameters = new List<Type>();

                // Iterate over the function parameters and infer types if needed
                for (var i = 0; i < typeFunc.Parameters.Length; i++)
                {
                    // Get the inferred parameter type for this call
                    var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                    // If needed, unify the types
                    visitor.Inferrer.UnifyTypesIfPossible(typeFunc.Parameters[i], inferredParamType.Type);

                    // Save the inferred type
                    parameters.Add(inferredParamType.Type);
                }

                // If the type is not yet infererd, assign a temporal one
                /*if (typeFunc.Return == null)
                    visitor.Inferrer.AssignTemporalTypeToSymbol(retSymbol);*/

                return new InferredType(typeFunc.Return);
            }
            else
            {
                // If the callable inferred type is not a Func, throw an exception
                // TODO: This could be replaced by structural typing, using the operator() method
                throw new System.Exception($"Cannot invoke a non-function object");
            }
        }
    }
}
