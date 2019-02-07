// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Zenit.Semantics.Symbols.Types.Specials
{
    /// <summary>
    /// Represents a referenced symbol that has not been
    /// resolved yet by the symbol resolver phase.
    /// Before moving to the next phase, all the unresolved references
    /// should be satisfied
    /// </summary>
    public interface IUnresolvedTypeSymbol : ISpecialTypeSymbol
    {
    }
}
