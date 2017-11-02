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
    class BlockNodeEvaluator : INodeEvaluator<AstEvaluator, AstBlockNode, Symbol>
    {
        public Symbol Evaluate(AstEvaluator evaluator, AstBlockNode node)
        {            
            evaluator.NewScope(ScopeType.Common);
            Symbol tmp = null;
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
