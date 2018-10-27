// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class BreakNode : Node
    {
        public Token Keyword { get; }
        public Node Number { get; }

        public BreakNode(Token keyword, Node number)
        {
            this.Keyword = keyword;
            this.Number = number;
        }
    }
}
