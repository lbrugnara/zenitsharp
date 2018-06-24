// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Syntax;

namespace Fl.Ast
{
    public class SymbolInformation
    {
        public Token Type { get; }
        public Token Mutability { get; }
        public Token Access { get; }
        public List<Token> Dimensions { get; }

        public SymbolInformation(Token type, Token mutability, Token accessModifier, List<Token> dimensions = null)
        {
            this.Type = type;
            this.Mutability = mutability;
            this.Access = accessModifier;
            this.Dimensions = dimensions;
        }
    }
}
