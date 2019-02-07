// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class UnaryNode : Node
    {
        public Token Operator { get; }
        public Node Left { get; }

        public UnaryNode(Token opToken, Node left)
        {
            this.Operator = opToken;
            this.Left = left;
        }
    }
}
