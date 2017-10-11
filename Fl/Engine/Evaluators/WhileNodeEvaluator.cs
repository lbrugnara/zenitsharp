// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class WhileNodeEvaluator : INodeEvaluator<AstEvaluator, AstWhileNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstWhileNode wnode)
        {
            evaluator.NewScope(ScopeType.Loop);
            try
            {
                ScopeEntry result = wnode.Condition.Exec(evaluator);
                if (!result.IsBool)
                    throw new AstWalkerException($"Cannot convert type {result.DataType} to {ScopeEntryType.Boolean}");
                while (result.BoolValue)
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
