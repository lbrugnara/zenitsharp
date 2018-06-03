// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using System.Collections.Generic;
using System.Linq;
using Fl.Lang.Types;
using Fl.Symbols;

namespace Fl.TypeChecker
{
    public class TypeChecker
    {
        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; }

        public TypeChecker(SymbolTable symtable)
        {
            this.SymbolTable = symtable ?? throw new System.ArgumentNullException(nameof(symtable));
        }

        public SymbolTable Check(AstNode node)
        {
            var visitor = new TypeCheckerVisitor(this.SymbolTable);
            visitor.Visit(node);
            return this.SymbolTable;
        }
    }
}
