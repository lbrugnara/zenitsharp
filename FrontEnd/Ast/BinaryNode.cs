// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class BinaryNode : Node
    {
        public Token Operator { get; }
        public Node Left { get; }
        public Node Right { get; }        

        public BinaryNode(Token opToken, Node left, Node right)
        {
            this.Operator = opToken;
            this.Left = left;
            this.Right = right;
        }
    }
}
