// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class WhileNodeEvaluator : INodeEvaluator<AstEvaluator, AstWhileNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstWhileNode wnode)
        {
            evaluator.Symtable.NewScope(ScopeType.Loop);
            try
            {
                FlObject result = wnode.Condition.Exec(evaluator);
                if (result.ObjectType != BoolType.Value)
                    throw new AstWalkerException($"Cannot convert type {result.ObjectType} to {BoolType.Value}");
                var boolResult = (result as FlBoolean);
                while (boolResult.Value)
                {
                    wnode.Body.Exec(evaluator);
                    if (evaluator.Symtable.MustBreak)
                        break;
                    if (evaluator.Symtable.MustContinue)
                        evaluator.Symtable.DoContinue();
                    boolResult = wnode.Condition.Exec(evaluator) as FlBoolean;
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
