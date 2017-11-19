// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Parser.Ast
{
    public class AstUnaryPostfixNode : AstUnaryNode
    {
        public AstUnaryPostfixNode(Token t, AstNode left)
            : base(t, left)
        {

        }
    }
}
