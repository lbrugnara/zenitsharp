// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public class AstBinaryNode : AstNode
    {
        public Token Operator { get; }
        public AstNode Left { get; }
        public AstNode Right { get; }        

        public AstBinaryNode(Token opToken, AstNode left, AstNode right)
        {
            Operator = opToken;
            Left = left;
            Right = right;
        }
    }
}
