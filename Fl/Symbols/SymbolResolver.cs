// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser.Ast;
using System.Collections.Generic;
using System.Linq;
using Fl.Lang.Types;
using Fl.Symbols;

namespace Fl.Symbols
{
    public class SymbolResolver : IAstWalker<Symbol>
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

            Package std = new Package("std", this.SymbolTable.CurrentBlock.Name);
            Package lang = std.NewPackage("lang");
            lang.NewSymbol("version", String.Instance);

            this.SymbolTable.AddSymbol(std);
        }

        // Adds a new block to the SymbolTable, it represents a new scope
        public void EnterBlock(BlockType type, string name)
        {
            this.SymbolTable.EnterBlock(type, name);
        }

        // Leave the current block in the SymbolTable
        public void LeaveBlock()
        {
            this.SymbolTable.LeaveBlock();
        }

        // Returns true if the current fragment is a function fragment
        public bool InFunction => this.SymbolTable.CurrentBlock.Type == BlockType.Function;

        public SymbolTable Resolve(AstNode node)
        {
            this.AstVisitor.Visit(node);
            return this.SymbolTable;
        }

        public Symbol Visit(AstNode node)
        {
            return this.AstVisitor.Visit(node);
        }
    }
}
