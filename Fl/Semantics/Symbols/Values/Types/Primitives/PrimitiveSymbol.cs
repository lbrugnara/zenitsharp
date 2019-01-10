﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class PrimitiveSymbol : TypeSymbol, IPrimitiveSymbol
    {
        public PrimitiveSymbol(BuiltinType type, ISymbolContainer parent)
            : base(type, parent)
        {
        }

        public override string ToString()
        {
            return this.BuiltinType.GetName();
        }

        public override string ToDebugString(int indent = 0)
        {
            return this.BuiltinType.GetName();
        }
    }
}
