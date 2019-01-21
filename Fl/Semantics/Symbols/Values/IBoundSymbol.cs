// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Symbols.Values
{
    /// <summary>
    /// Represents a value symbol that is bound to a variable
    /// </summary>
    public interface IBoundSymbol : ISymbol
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

        /// <summary>
        /// Change the symbol's underlying type
        /// </summary>
        /// <param name="type"></param>
        void ChangeType(ITypeSymbol type);
    }
}
