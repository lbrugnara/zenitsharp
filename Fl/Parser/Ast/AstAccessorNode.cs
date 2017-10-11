// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;

namespace Fl.Parser.Ast
{
    public class AstAccessorNode : AstNode
    {
        public Token Self;
        public AstNode Member;

        public AstAccessorNode(Token self, AstNode member)
        {
            Self = self;
            Member = member;
        }
    }
}
