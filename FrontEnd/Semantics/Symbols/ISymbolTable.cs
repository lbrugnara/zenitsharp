﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Variables;

namespace Zenit.Semantics.Symbols
{
    public interface ISymbolTable
    {
        void LeaveScope();

        /// <summary>
        /// Creates and inserts into the symbol table a new Symbol
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="access"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        IVariable AddNewVariableSymbol(string name, IType type, Access access, Storage storage);

        /// <summary>
        /// Return true if a symbol with that name exists in the
        /// symbol table
        /// </summary>
        /// <param name="name">Symbol's name to lookup</param>
        /// <returns>True if symbols exist</returns>
        bool HasVariableSymbol(string name);

        /// <summary>
        /// Return a symbol with the provided name. It should throw
        /// if the symbol does not exist in the symbol table
        /// </summary>
        /// <param name="name">Symbol's name to retrieve</param>
        /// <returns>Symbol instance identified by name</returns>
        IVariable GetVariableSymbol(string name);
    }
}
