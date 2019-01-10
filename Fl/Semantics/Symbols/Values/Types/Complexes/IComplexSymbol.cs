// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Values;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public interface IComplexSymbol : ITypeSymbol, ISymbolContainer
    {
        /// <summary>
        /// Type's properties
        /// </summary>
        Dictionary<string, IValueSymbol> Properties { get; }

        /// <summary>
        /// Type's functions
        /// </summary>
        Dictionary<string, IValueSymbol> Functions { get; }
    }
}
