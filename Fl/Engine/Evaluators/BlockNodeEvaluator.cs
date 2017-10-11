// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class BlockNodeEvaluator : INodeEvaluator<AstEvaluator, AstBlockNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstBlockNode node)
        {            
            evaluator.NewScope(ScopeType.Common);
            ScopeEntry tmp = null;
            try
            {
                foreach (AstNode statement in node.Statements)
                {
                    tmp = statement.Exec(evaluator);
                    if (evaluator.CurrentScope.MustBreak || evaluator.CurrentScope.MustContinue || evaluator.CurrentScope.MustReturn)
                        break;                    
                }
            }
            finally
            {
                evaluator.DestroyScope();
            }
            return tmp;
        }
    }
}
