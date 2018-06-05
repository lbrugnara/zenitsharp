// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols;
using Fl.TypeChecking.Inferrers;
using Fl.TypeChecking.Checkers;

namespace Fl.TypeChecking
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
            var inferencer = new TypeInferrerVisitor(this.SymbolTable);
            inferencer.Visit(node);

            var checker = new TypeCheckerVisitor(this.SymbolTable);
            checker.Visit(node);

            return this.SymbolTable;
        }
    }
}
