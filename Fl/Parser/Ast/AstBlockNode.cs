// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Parser.Ast
{
    public class AstBlockNode : AstNode
    {
        public List<AstNode> Statements { get; }

        public AstBlockNode(List<AstNode> statements)
        {
            Statements = statements;
        }
    }
}
