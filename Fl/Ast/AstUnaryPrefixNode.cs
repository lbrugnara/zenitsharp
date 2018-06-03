﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser;

namespace Fl.Ast
{
    public class AstUnaryPrefixNode : AstUnaryNode
    {
        public AstUnaryPrefixNode(Token opToken, AstNode left)
            : base (opToken, left)
        {

        }
    }
}