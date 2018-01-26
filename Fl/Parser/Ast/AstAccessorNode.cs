// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Parser.Ast
{
    public class AstAccessorNode : AstNode
    {
        public Token Identifier;
        public AstNode Enclosing;

        public AstAccessorNode(Token identifier, AstNode enclosing)
        {
            Identifier = identifier;
            Enclosing = enclosing;
        }
    }
}
