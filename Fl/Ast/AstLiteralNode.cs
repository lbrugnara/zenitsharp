// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser;

namespace Fl.Ast
{
    public class AstLiteralNode : AstNode
    {
        public Token Literal { get; }

        public AstLiteralNode(Token t)
        {
            this.Literal = t;
        }

        public override string ToString()
        {
            return this.Literal.Value.ToString();
        }
    }
}
