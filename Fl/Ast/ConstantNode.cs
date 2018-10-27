// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using Fl.Syntax;

namespace Fl.Ast
{
    public class ConstantNode : Node
    {
        public Token Type { get; }
        public List<SymbolDefinition> Definitions { get; }

        public ConstantNode(Token type, List<SymbolDefinition> definitions)
        {
            this.Type = type;
            this.Definitions = definitions;
        }
    }
}
