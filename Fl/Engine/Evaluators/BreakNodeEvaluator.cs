// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class BreakNodeEvaluator : INodeEvaluator<AstEvaluator, AstBreakNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator evaluator, AstBreakNode wnode)
        {
            Symbol breakval = wnode.Number?.Exec(evaluator) ?? new Symbol(SymbolType.Integer, 1);
            if (!breakval.IsInt)
                throw new AstWalkerException($"Break expression only allows integer values");
            evaluator.CurrentScope.SetBreak(breakval.AsInt);
            return breakval;
        }
    }
}
