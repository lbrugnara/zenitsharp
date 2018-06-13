// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;
using Fl.Symbols.Resolvers;

namespace Fl.Symbols
{
    public class SymbolResolver
    {
        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; }

        public SymbolResolver()
        {
            this.SymbolTable = new SymbolTable();

            /*Package std = new Package("std", this.SymbolTable.Scope.Uid);
            Package lang = std.NewPackage("lang");
            lang.NewSymbol("version", String.Instance);

            this.SymbolTable.AddSymbol(std);*/
        }

        public SymbolTable Resolve(AstNode node)
        {
            var visitor = new SymbolResolverVisitor(this.SymbolTable);
            visitor.Visit(node);
            return this.SymbolTable;
        }
    }
}
