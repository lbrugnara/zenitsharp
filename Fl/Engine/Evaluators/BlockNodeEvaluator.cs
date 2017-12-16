// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class BlockNodeEvaluator : INodeVisitor<AstEvaluator, AstBlockNode, FlObject>
    {
        public FlObject Visit(AstEvaluator evaluator, AstBlockNode node)
        {            
            evaluator.Symtable.EnterScope(ScopeType.Common);
            FlObject tmp = null;
            try
            {
                foreach (AstNode statement in node.Statements)
                {
                    tmp = statement.Exec(evaluator);
                    if (evaluator.Symtable.MustBreak || evaluator.Symtable.MustContinue || evaluator.Symtable.MustReturn)
                        break;                    
                }
            }
            finally
            {
                evaluator.Symtable.LeaveScope();
            }
            return tmp;
        }
    }
}
