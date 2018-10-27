// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class LiteralNode : Node
    {
        public Token Literal { get; }

        public LiteralNode(Token t)
        {
            this.Literal = t;
        }

        public override string ToString()
        {
            return this.Literal.Value;
        }
    }
}
