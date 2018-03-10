// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public class AstNullCoalescingNode : AstNode
    {
        public Token Keyword { get; }
        public AstNode Left { get; }
        public AstNode Right { get; }

        public AstNullCoalescingNode(Token keyword, AstNode left, AstNode right)
        {
            this.Keyword = keyword;
            this.Left = left;
            this.Right = right;
        }
    }
}
