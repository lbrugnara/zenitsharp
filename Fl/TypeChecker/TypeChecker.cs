// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using System.Collections.Generic;
using System.Linq;
using Fl.Lang.Types;
using Fl.Symbols;
using System;

namespace Fl.TypeChecker
{
    public class TypeChecker : IAstWalker<Symbol>
    {
        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; }

        /// <summary>
        /// All the visitor logic is moved to the AstVisitor
        /// </summary>
        private AstVisitor AstVisitor { get; }

        public TypeChecker(SymbolTable symtable)
        {
            this.SymbolTable = symtable ?? throw new ArgumentNullException(nameof(symtable));
            this.AstVisitor = new AstVisitor(this);            
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

        public Symbol Visit(AstNode node)
        {
            return this.AstVisitor.Visit(node);
        }
    }
}
