// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class WhileNodeEvaluator : INodeEvaluator<AstEvaluator, AstWhileNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator evaluator, AstWhileNode wnode)
        {
            evaluator.NewScope(ScopeType.Loop);
            try
            {
                Symbol result = wnode.Condition.Exec(evaluator);
                if (!result.IsBool)
                    throw new AstWalkerException($"Cannot convert type {result.DataType} to {SymbolType.Boolean}");
                while (result.AsBool)
                {
                    wnode.Body.Exec(evaluator);
                    if (evaluator.CurrentScope.MustBreak)
                        break;
                    if (evaluator.CurrentScope.MustContinue)
                        evaluator.CurrentScope.DoContinue();
                    result = wnode.Condition.Exec(evaluator);
                }
            }
            finally
            {
                evaluator.DestroyScope();
            }
            return null;
        }
    }
}
