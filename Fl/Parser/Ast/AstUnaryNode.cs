// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser.Ast
{
    public class AstUnaryNode : AstNode
    {
        public Token Operator { get; }
        public AstNode Left { get; }

        public AstUnaryNode(Token t, AstNode left)
        {
            Operator = t;
            Left = left;
        }
    }
}
