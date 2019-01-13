// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols
{
    /// <summary>
    /// Represents any type of symbol that can be added to an ISymbolTable
    /// </summary>
    public interface ISymbol
    {
        /// <summary>
        /// Name of the entry that indentifies it in the symbol table
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Reference to the parent scope, if present
        /// </summary>
        ISymbolContainer Parent { get; }
    }
}
