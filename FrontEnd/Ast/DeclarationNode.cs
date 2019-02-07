// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Zenit.Ast
{
    public class DeclarationNode : Node
    {
        public List<Node> Statements { get; }

        public DeclarationNode(List<Node> statements)
        {
            this.Statements = statements;
        }
    }
}
