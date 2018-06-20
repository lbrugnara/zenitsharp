// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstBinaryNode : AstNode
    {
        public Token Operator { get; }
        public AstNode Left { get; }
        public AstNode Right { get; }        

        public AstBinaryNode(Token opToken, AstNode left, AstNode right)
        {
            this.Operator = opToken;
            this.Left = left;
            this.Right = right;
        }
    }
}
