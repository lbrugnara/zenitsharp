// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Syntax;

namespace Fl.Ast
{
    public class AstTypeNode : AstNode
    {
        public Token Name { get; }
        public List<Token> Dimensions { get; }

        public AstTypeNode(Token type, List<Token> dimensions = null)
        {
            this.Name = type;
            this.Dimensions = dimensions;
        }
    }
}
