// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Parser.Ast
{
    public class AstConstantNode : AstNode
    {
        public Token Type { get; }
        public List<Tuple<Token, AstNode>> Constants { get; }

        public AstConstantNode(Token type, List<Tuple<Token, AstNode>> constdefs)
        {
            Type = type;
            Constants = constdefs;
        }
    }
}
