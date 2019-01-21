// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols.Types
{
    /// <summary>
    /// Represents a built-in type symbol
    /// </summary>
    public interface ITypeSymbol : ISymbol
    {
        BuiltinType BuiltinType { get; }
    }
}
