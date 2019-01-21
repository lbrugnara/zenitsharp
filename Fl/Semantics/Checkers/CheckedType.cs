﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Checkers
{
    public class CheckedType
    {
        public ITypeSymbol TypeSymbol { get; set; }
        public IBoundSymbol Symbol { get; set; }

        public CheckedType(ITypeSymbol type, IBoundSymbol symbol = null)
        {
            this.TypeSymbol = type;
            this.Symbol = symbol;
        }
    }
}
