// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;

namespace Fl.Engine.Evaluators
{
    class ReturnNodeEvaluator : INodeEvaluator<AstEvaluator, AstReturnNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator evaluator, AstReturnNode wnode)
        {
            Symbol retval = wnode.Expression?.Exec(evaluator) ?? new Symbol(SymbolType.Null, null);
            evaluator.CurrentScope.ReturnValue = retval;
            return retval;
        }
    }
}
