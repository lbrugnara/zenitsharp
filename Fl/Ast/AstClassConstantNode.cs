// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using Fl.Syntax;

namespace Fl.Ast
{
    public class AstClassConstantNode : AstNode
    {
        public Token Name { get; }
        public Token AccessModifier { get; }
        public Token Type { get; }
        public AstNode Definition { get; }

        public AstClassConstantNode(Token name, Token accessModifier, Token type, AstNode definition)
        {
            this.Name = Name;
            this.AccessModifier = accessModifier;
            this.Type = type;
            this.Definition = definition;
        }
    }
}
