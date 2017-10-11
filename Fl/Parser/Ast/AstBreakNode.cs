// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser.Ast
{
    public class AstBreakNode : AstNode
    {
        public Token Keyword { get; }
        public AstNode Number { get; }

        public AstBreakNode(Token keyword, AstNode number)
        {
            Keyword = keyword;
            Number = number;
        }
    }
}
