// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class SymbolDefinition
    {
        public Token Left { get; }
        public Node Right { get; }

        public SymbolDefinition(Token left, Node right)
        {
            this.Left = left;
            this.Right = right;
        }
    }
}
