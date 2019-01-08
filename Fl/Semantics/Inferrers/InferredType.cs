﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    public class InferredType
    {
        public TypeInfo TypeInfo { get; set; }
        public ISymbol Symbol { get; set; }

        public InferredType(TypeInfo type, ISymbol symbol = null)
        {
            this.TypeInfo = type;
            this.Symbol = symbol;
        }
    }
}
