// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Types;

namespace Zenit.Semantics.Symbols.Types
{
    /// <summary>
    /// Represents a built-in type symbol
    /// </summary>
    public interface IType : ISymbol
    {
        BuiltinType BuiltinType { get; }
    }
}
