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
        public InferredType Visit(TypeInferrerVisitor checker, AstCallableNode node)
        {
            var target = node.Callable.Visit(checker);

            if (!(target.Type is Func))
                throw new System.Exception($"Cannot invoke a non-function object");

            var function = checker.SymbolTable.GetSymbol(target.Symbol) as Function;

            if (function.Parameters.Length != node.Arguments.Expressions.Count)
                throw new System.Exception($"Function {function.Name} expects {function.Parameters.Length} but received {node.Arguments.Expressions.Count}");

            var parameters = new List<Type>();

            for (var i=0; i < function.Parameters.Length; i++)
            {
                var p = function.GetSymbol(function.Parameters[i]);
                var inferredParamType = node.Arguments.Expressions[i].Visit(checker);

                parameters.Add(inferredParamType.Type);

                if (p.Type is Anonymous && !(inferredParamType.Type is Anonymous))
                {
                    checker.Constraints.InferTypeAs(p.Type as Anonymous, inferredParamType.Type);
                }
                else if (inferredParamType.Type is Anonymous && !(p.Type is Anonymous))
                {
                    checker.Constraints.InferTypeAs(inferredParamType.Type as Anonymous, p.Type);
                }
            }

            if (checker.Constraints.HasConstraints(function))
            {
                var retSymbol = function.GetSymbol("@ret");

                checker.Constraints.ResolveConstraint(function, new Func(parameters.ToArray(), retSymbol.Type));
            }

            return new InferredType((function.Type as Func).Return);
        }
    }
}
