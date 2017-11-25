// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Evaluators
{
    public class CallableNodeEvaluator : INodeEvaluator<AstEvaluator, AstCallableNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstCallableNode node)
        {
            FlObject target = node.Callable.Exec(evaluator);

            if (target is FlClass)
            {
                var clasz = (target as FlClass);

                if (node.New != null)
                {
                    var newInstance = clasz.Activator.Invoke();
                    target = clasz.GetConstructor(node.Arguments.Count);
                    if (target == null)
                    {                        
                        if (newInstance == null)
                            throw new AstWalkerException($"{clasz} does not contain a constructor that accepts {node.Arguments.Count}");
                        return newInstance;
                    }
                    return (target as FlConstructor).Bind(newInstance).Invoke(evaluator.Symtable, node.Arguments.Expressions.Select(a => a.Exec(evaluator)).ToList());
                }
                else
                { 
                    target = clasz.StaticConstructor ?? throw new AstWalkerException($"{target} does not contain a definition for the static constructor");
                }
            }

            if (target is FlFunction)
            {
                return (target as FlFunction).Invoke(evaluator.Symtable, node.Arguments.Expressions.Select(a => a.Exec(evaluator)).ToList());
            }
            throw new AstWalkerException($"{target} is not a callable object");
        }
    }
}
