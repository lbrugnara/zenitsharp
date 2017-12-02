// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Text;

namespace Fl.Parser.Ast
{
    public class AstConstantNode : AstNode
    {
        public Token Type { get; }
        public Token Identifier { get; }
        public AstNode Initializer { get; }

        public AstConstantNode(Token identifier, AstNode initializer, Token type = null)
        {
            Type = type;
            Identifier = identifier;
            Initializer = initializer;
        }
    }
}
