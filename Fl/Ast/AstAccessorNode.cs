// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser;

namespace Fl.Ast
{
    public class AstAccessorNode : AstNode
    {
        public Token Identifier;
        public AstNode Enclosing;

        public AstAccessorNode(Token identifier, AstNode enclosing)
        {
            this.Identifier = identifier;
            this.Enclosing = enclosing;
        }
    }
}
