// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols
{
    public interface ISymbolTableEntry
    {
        /// <summary>
        /// Name of the entry that indentifies it in the symbol table
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Reference to the parent scope, if present
        /// </summary>
        ISymbolContainer Parent { get; }

        /// <summary>
        /// String with a dump of the entry
        /// </summary>
        /// <param name="indent">Indentation of the members of the entry</param>
        /// <returns></returns>
        string ToDebugString(int indent = 0);
    }
}
