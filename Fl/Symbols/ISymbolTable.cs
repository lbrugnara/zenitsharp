// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols.Types;

namespace Fl.Symbols
{
    public interface ISymbolTable
    {
        /// <summary>
        /// Adds symbol to this SymbolTable
        /// </summary>
        /// <param name="symbol"></param>
        void AddSymbol(Symbol symbol);

        /// <summary>
        /// Create a new symbol, add it to the symbol table
        /// and return the symbol to the caller
        /// </summary>
        /// <param name="name">Symbol's name</param>
        /// <param name="type">Symbol's data type</param>
        /// <returns></returns>
        Symbol NewSymbol(string name, Type type);

        /// <summary>
        /// Return true if a symbol with that name exists in the
        /// symbol table
        /// </summary>
        /// <param name="name">Symbol's name to lookup</param>
        /// <returns>True if symbols exist</returns>
        bool HasSymbol(string name);

        /// <summary>
        /// Return a symbol with the provided name. It should throw
        /// if the symbol does not exist in the symbol table
        /// </summary>
        /// <param name="name">Symbol's name to retrieve</param>
        /// <returns>Symbol instance identified by name</returns>
        Symbol GetSymbol(string name);

        /// <summary>
        /// Return a symbol with the provided name. It returns null
        /// if the symbol does not exist in the symbol table
        /// </summary>
        /// <param name="name">Symbol's name to retrieve</param>
        /// <returns>Symbol instance identified by name</returns>
        Symbol TryGetSymbol(string name);
    }
}
