// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    /// <summary>
    /// Represents a value symbol that is bound to a variable
    /// </summary>
    public interface IBoundSymbol : IValueSymbol
    {
        /// <summary>
        /// Bound symbol's type
        /// </summary>
        ITypeSymbol TypeSymbol { get; }

        /// <summary>
        /// Access level to the bound variable
        /// </summary>
        Access Access { get; }

        /// <summary>
        /// Storage type of the bound variable
        /// </summary>
        Storage Storage { get; }
    }
}
