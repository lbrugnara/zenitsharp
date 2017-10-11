// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser.Ast
{
    public class AstReturnNode : AstNode
    {
        public Token Keyword { get; }
        public AstNode Expression { get; }

        public AstReturnNode(Token keyword, AstNode expr)
        {
            Keyword = keyword;
            Expression = expr;
        }
    }
}
