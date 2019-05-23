// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class PrimitiveNode : Node
    {
        public Token Literal { get; }

        public PrimitiveNode(Token t)
        {
            this.Literal = t;
        }

        public override string ToString()
        {
            return this.Literal.Value;
        }
    }
}
