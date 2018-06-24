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
        public SymbolInformation SymbolInfo { get; }
        public AstNode Definition { get; }

        public AstClassConstantNode(Token name, SymbolInformation modifiers, AstNode definition)
        {
            this.Name = name;
            this.SymbolInfo = modifiers;
            this.Definition = definition;
        }
    }
}
