using Fl.Ast;
using Fl.Semantics.Resolvers;
using Fl.Semantics.Checkers;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Semantics
{
    public class SemanticAnalysis
    {
        private TypeInferrer typeInferrer;
        private SymbolTable symbolTable;
        private SymbolResolverVisitor resolver;
        private TypeInferrerVisitor inferrer;
        private TypeCheckerVisitor checker;

        public SemanticAnalysis()
        {
            this.typeInferrer = new TypeInferrer();
            this.symbolTable = new SymbolTable();

            this.resolver = new SymbolResolverVisitor(this.symbolTable, this.typeInferrer);
            this.inferrer = new TypeInferrerVisitor(this.symbolTable, this.typeInferrer);
            this.checker = new TypeCheckerVisitor(this.symbolTable);

            /*var intClass = new Class();

            this.SymbolTable.NewSymbol("int", intClass);
            this.SymbolTable.EnterClassScope("int");
            this.SymbolTable.NewSymbol("toStr", new Function(String.Instance));
            this.SymbolTable.LeaveScope();*/
        }        

        public SymbolTable Run(AstNode ast)
        {
            this.resolver.Visit(ast);

            // If there are unresolved classes, throw an exception
            this.symbolTable.ThrowIfUnresolved();

            this.inferrer.Visit(ast);
            this.checker.Visit(ast);

            return this.symbolTable;
        }
    }
}
