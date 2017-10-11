// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class DeclarationNodeEvaluator : INodeEvaluator<AstEvaluator, AstDeclarationNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstDeclarationNode decls)
        {
            ScopeEntry tmp = null;
            foreach (AstNode statement in decls.Statements)
            {
                tmp = statement.Exec(evaluator);
            }
            return tmp;
        }
    }
}
