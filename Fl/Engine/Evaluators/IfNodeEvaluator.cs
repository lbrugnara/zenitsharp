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
    class IfNodeEvaluator : INodeEvaluator<AstEvaluator, AstIfNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstIfNode ifnode)
        {
            evaluator.Symtable.NewScope(ScopeType.Common);
            try
            {
                FlObject result = ifnode.Condition.Exec(evaluator);
                if (!result.IsBool)
                    throw new AstWalkerException($"Cannot convert type {result.Type} to {ObjectType.Boolean}");
                if (result.AsBool)
                    return ifnode.Then?.Exec(evaluator);
                if (ifnode.Else != null)
                    return ifnode.Else.Exec(evaluator);
            }
            finally
            {
                evaluator.Symtable.DestroyScope();
            }
            return null;
        }
    }
}
