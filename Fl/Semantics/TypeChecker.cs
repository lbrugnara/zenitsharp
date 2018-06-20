// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Checkers;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;

namespace Fl.Semantics
{
    public class TypeChecker
    {
        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; }

        public TypeInferrer TypeInferrer { get; }

        public TypeChecker(SymbolTable symtable, TypeInferrer inferrer)
        {
            this.SymbolTable = symtable ?? throw new System.ArgumentNullException(nameof(symtable));
            this.TypeInferrer = inferrer ?? throw new System.ArgumentNullException(nameof(inferrer));
        }

        public SymbolTable Check(AstNode node)
        {
            var inferencer = new TypeInferrerVisitor(this.SymbolTable, this.TypeInferrer);
            inferencer.Visit(node);

            var checker = new TypeCheckerVisitor(this.SymbolTable);
            checker.Visit(node);

            return this.SymbolTable;
        }
    }
}
