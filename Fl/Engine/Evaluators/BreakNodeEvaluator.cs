// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class BreakNodeEvaluator : INodeEvaluator<AstEvaluator, AstBreakNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstBreakNode wnode)
        {
            ScopeEntry breakval = wnode.Number?.Exec(evaluator) ?? new ScopeEntry(ScopeEntryType.Integer, 1);
            if (!breakval.IsInt)
                throw new AstWalkerException($"Break expression only allows integer values");
            evaluator.CurrentScope.SetBreak(breakval.IntValue);
            return breakval;
        }
    }
}
