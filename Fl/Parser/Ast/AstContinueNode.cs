// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser.Ast
{
    public class AstContinueNode : AstNode
    {
        public Token Keyword { get; }

        public AstContinueNode(Token keyword)
        {
            Keyword = keyword;
        }
    }
}
