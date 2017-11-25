// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class ForNodeEvaluator : INodeEvaluator<AstEvaluator, AstForNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstForNode fornode)
        {
            evaluator.Symtable.NewScope(ScopeType.Loop);
            try
            {
                fornode.Init.Exec(evaluator);
                FlObject result = fornode.Condition.Exec(evaluator);
                if (result != null && result.ObjectType != BoolType.Value)
                    throw new AstWalkerException($"Cannot convert type {result.ObjectType} to {BoolType.Value}");
                while (result == null || (result as FlBool).Value)
                {
                    fornode.Body.Exec(evaluator);
                    if (evaluator.Symtable.MustBreak)
                        break;
                    fornode.Increment.Exec(evaluator);
                    result = fornode.Condition.Exec(evaluator);
                }
            }
            finally
            {
                evaluator.Symtable.DestroyScope();
            }
            return null;
        }
    }
}
