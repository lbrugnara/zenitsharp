// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstContinueNode : AstNode
    {
        public Token Keyword { get; }

        public AstContinueNode(Token keyword)
        {
            this.Keyword = keyword;
        }
    }
}
