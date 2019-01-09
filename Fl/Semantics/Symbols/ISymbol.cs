// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public interface ISymbol : ISymbolTableEntry
    {
        TypeInfo TypeInfo { get; }
        Access Access { get; }
        Storage Storage { get; }
    }
}
