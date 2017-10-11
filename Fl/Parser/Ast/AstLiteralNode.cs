// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser.Ast
{
    public class AstLiteralNode : AstNode
    {
        public Token Primary { get; }

        public AstLiteralNode(Token t)
        {
            Primary = t;
        }
    }
}
