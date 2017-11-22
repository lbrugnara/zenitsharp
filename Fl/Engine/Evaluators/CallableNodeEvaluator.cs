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
            if (target.ObjectType != FunctionType.Value)
                throw new AstWalkerException($"{target.ToString()} is not a callable object");
            return (target as FlCallable).Invoke(evaluator.Symtable, node.Arguments.Expressions.Select(a => a.Exec(evaluator)).ToList());
        }
    }
}
