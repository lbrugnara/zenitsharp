// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstBreakNode : AstNode
    {
        public Token Keyword { get; }
        public AstNode Number { get; }

        public AstBreakNode(Token keyword, AstNode number)
        {
            this.Keyword = keyword;
            this.Number = number;
        }
    }
}
