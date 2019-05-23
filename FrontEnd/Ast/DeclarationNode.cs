// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Zenit.Ast
{
    public class DeclarationNode : Node
    {
        public List<Node> Declarations { get; }

        public DeclarationNode(List<Node> declarations)
        {
            this.Declarations = declarations;
        }
    }
}
