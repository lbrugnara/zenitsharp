// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class IfNodeEvaluator : INodeEvaluator<AstEvaluator, AstIfNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstIfNode ifnode)
        {
            evaluator.NewScope(ScopeType.Common);
            try
            {
                ScopeEntry result = ifnode.Condition.Exec(evaluator);
                if (!result.IsBool)
                    throw new AstWalkerException($"Cannot convert type {result.DataType} to {ScopeEntryType.Boolean}");
                if (result.BoolValue)
                    return ifnode.Then != null ? ifnode.Then.Exec(evaluator) : null;
                if (ifnode.Else != null)
                    return ifnode.Else.Exec(evaluator);
            }
            finally
            {
                evaluator.DestroyScope();
            }
            return null;
        }
    }
}
