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
    class DeclarationNodeEvaluator : INodeVisitor<AstEvaluator, AstDeclarationNode, FlObject>
    {
        public FlObject Visit(AstEvaluator evaluator, AstDeclarationNode decls)
        {
            FlObject tmp = null;
            foreach (AstNode statement in decls.Statements)
            {
                tmp = statement.Exec(evaluator);
            }
            return tmp;
        }
    }
}
