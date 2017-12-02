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

            if (node is AstIndexerNode)
            {
                Symbol clasz = evaluator.Symtable.GetSymbol(target.ObjectType.ClassName) ?? evaluator.Symtable.GetSymbol(target.ObjectType.Name);
                var claszobj = (clasz.Binding as FlClass);
                FlIndexer indexer = claszobj.GetIndexer(node.Arguments.Count);
                if (indexer == null)
                    throw new AstWalkerException($"{claszobj} does not contain an indexer that accepts {node.Arguments.Count} {(node.Arguments.Count == 1 ? "argument" : "arguments")}");
                return indexer.Bind(target).Invoke(evaluator.Symtable, node.Arguments.Expressions.Select(e => e.Exec(evaluator)).ToList());
            }

            if (target is FlClass)
            {
                var clasz = (target as FlClass);

                if (node.New != null)
                {
                    return clasz.InvokeConstructor(node.Arguments.Expressions.Select(a => a.Exec(evaluator)).ToList());
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
