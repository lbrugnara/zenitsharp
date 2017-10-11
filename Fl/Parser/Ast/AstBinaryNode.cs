// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public class AstBinaryNode : AstNode
    {
        public Token Operator { get; }
        public AstNode Left { get; }
        public AstNode Right { get; }        

        public AstBinaryNode(Token t, AstNode left, AstNode right)
        {
            Operator = t;
            Left = left;
            Right = right;
        }
    }
}
