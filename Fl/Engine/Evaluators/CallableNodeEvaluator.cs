// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Evaluators
{
    public class CallableNodeEvaluator : INodeEvaluator<AstEvaluator, AstCallableNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator evaluator, AstCallableNode node)
        {
            Symbol target = node.Callable.Exec(evaluator);
            if (!target.IsCallable)
                throw new AstWalkerException($"{target.ToString()} is not a callable object");
            return target.AsCallable.Invoke(evaluator, node.Arguments.Expressions.Select(a => a.Exec(evaluator)).ToList());
        }
    }
}
