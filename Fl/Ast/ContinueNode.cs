// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
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
