// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public interface ISymbolTable
    {
        /// <summary>
        /// Adds symbol to this SymbolTable bound to the specific name
        /// </summary>
        /// <param name="symbol"></param>
        void Insert(string name, IBoundSymbol symbol);

        /// <summary>
        /// Creates and inserts into the symbol table a new Symbol
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="access"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        IBoundSymbol Insert(string name, ITypeSymbol type, Access access, Storage storage);

        /// <summary>
        /// Removes the symbol name from the table
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);

        /// <summary>
        /// Return true if a symbol with that name exists in the
        /// symbol table
        /// </summary>
        /// <param name="name">Symbol's name to lookup</param>
        /// <returns>True if symbols exist</returns>
        bool Contains(string name);

        /// <summary>
        /// Return a symbol with the provided name. It should throw
        /// if the symbol does not exist in the symbol table
        /// </summary>
        /// <param name="name">Symbol's name to retrieve</param>
        /// <returns>Symbol instance identified by name</returns>
        IBoundSymbol GetBoundSymbol(string name);

        /// <summary>
        /// Return a symbol with the provided name. It returns null
        /// if the symbol does not exist in the symbol table
        /// </summary>
        /// <param name="name">Symbol's name to retrieve</param>
        /// <returns>Symbol instance identified by name</returns>
        IBoundSymbol TryGetBoundSymbol(string name);
    }
}
