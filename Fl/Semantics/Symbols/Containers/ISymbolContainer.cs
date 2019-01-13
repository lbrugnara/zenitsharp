// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols
{
    public interface ISymbolContainer : ISymbol
    {
        /// <summary>
        /// Adds symbol to this SymbolTable bound to the specific name
        /// </summary>
        /// <param name="symbol"></param>
        void Insert<T>(string name, T symbol) where T : ISymbol;

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
        T Get<T>(string name) where T : ISymbol;

        /// <summary>
        /// Return a symbol with the provided name. It returns null
        /// if the symbol does not exist in the symbol table
        /// </summary>
        /// <param name="name">Symbol's name to retrieve</param>
        /// <returns>Symbol instance identified by name</returns>
        T TryGet<T>(string name) where T : ISymbol;

        /// <summary>
        /// String with a dump of the entry
        /// </summary>
        /// <param name="indent">Indentation of the members of the entry</param>
        /// <returns></returns>
        string ToDumpString(int indent = 0);
    }
}
