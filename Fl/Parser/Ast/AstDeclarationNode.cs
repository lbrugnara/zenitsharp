// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Parser.Ast
{
    public class AstDeclarationNode : AstNode
    {
        public List<AstNode> Statements { get; }

        public AstDeclarationNode(List<AstNode> statements)
        {
            this.Statements = statements;
        }
    }
}
