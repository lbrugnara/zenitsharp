// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using System.Collections.Generic;
using System.Linq;
using Fl.Lang.Types;
using Fl.Symbols;

namespace Fl.Symbols
{
    public class SymbolResolver : IAstWalker
    {
        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; }

        /// <summary>
        /// All the visitor logic is moved to the AstVisitor
        /// </summary>
        private AstVisitor AstVisitor { get; }

        public SymbolResolver()
        {            
            this.AstVisitor = new AstVisitor(this);

            this.SymbolTable = new SymbolTable();

            Package std = new Package("std", this.SymbolTable.CurrentBlock.Uid);
            Package lang = std.NewPackage("lang");
            lang.NewSymbol("version", String.Instance);

            this.SymbolTable.AddSymbol(std);
        }

        public SymbolTable Resolve(AstNode node)
        {
            this.AstVisitor.Visit(node);
            return this.SymbolTable;
        }

        public void Visit(AstNode node)
        {
            this.AstVisitor.Visit(node);
        }
    }
}
