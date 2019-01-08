// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols
{
    public interface ISymbolContainer : ISymbolTableEntry
    {
        /// <summary>
        /// Adds symbol to this SymbolTable
        /// </summary>
        /// <param name="symbol"></param>
        void Insert<T>(T symbol) where T : ISymbolTableEntry;

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
        T Get<T>(string name) where T : ISymbolTableEntry;

        /// <summary>
        /// Return a symbol with the provided name. It returns null
        /// if the symbol does not exist in the symbol table
        /// </summary>
        /// <param name="name">Symbol's name to retrieve</param>
        /// <returns>Symbol instance identified by name</returns>
        T TryGet<T>(string name) where T : ISymbolTableEntry;
    }
}
