// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class ForNodeEvaluator : INodeEvaluator<AstEvaluator, AstForNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstForNode fornode)
        {
            evaluator.NewScope(ScopeType.Loop);
            try
            {
                fornode.Init.Exec(evaluator);
                ScopeEntry result = fornode.Condition.Exec(evaluator);
                if (!result.IsBool)
                    throw new AstWalkerException($"Cannot convert type {result.DataType} to {ScopeEntryType.Boolean}");
                while (result.BoolValue)
                {
                    fornode.Body.Exec(evaluator);
                    if (evaluator.CurrentScope.MustBreak)
                        break;
                    fornode.Increment.Exec(evaluator);
                    result = fornode.Condition.Exec(evaluator);
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
