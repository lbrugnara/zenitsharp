// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class ContinueNode : Node
    {
        public Token Keyword { get; }

        public ContinueNode(Token keyword)
        {
            this.Keyword = keyword;
        }
    }
}
