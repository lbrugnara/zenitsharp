// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    /// <summary>
    /// Represents a built-in type symbol
    /// </summary>
    public interface ITypeSymbol : IValueSymbol
    {
        BuiltinType BuiltinType { get; }
    }
}
