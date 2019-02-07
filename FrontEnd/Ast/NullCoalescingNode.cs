// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class NullCoalescingNode : Node
    {
        public Token Keyword { get; }
        public Node Left { get; }
        public Node Right { get; }

        public NullCoalescingNode(Token keyword, Node left, Node right)
        {
            this.Keyword = keyword;
            this.Left = left;
            this.Right = right;
        }
    }
}
