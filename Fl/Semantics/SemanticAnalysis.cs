using Fl.Ast;
using Fl.Semantics.Resolvers;
using Fl.Semantics.Checkers;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using System;
using Fl.Semantics.Mutability;
using Fl.Helpers;

namespace Fl.Semantics
{
    public class SemanticAnalysis
    {
        /// <summary>
        /// The type inferrer keeps track of unresolved types and granting anonymous types
        /// that will be replaced/updated once the types are resolved
        /// </summary>
        private TypeInferrer typeInferrer;

        /// <summary>
        /// It keeps track of the scopes and symbols declared on each of them
        /// </summary>
        private SymbolTable symbolTable;

        /// <summary>
        /// The symbol resolver will visit each node and will add scopes and symbols within them
        /// </summary>
        private SymbolResolverVisitor resolver;

        /// <summary>
        /// The type inferrer will try to conclude the type of every expression in the program
        /// </summary>
        private TypeInferrerVisitor inferrer;

        /// <summary>
        /// It makes sure all the operations are made between compatible types
        /// </summary>
        private TypeCheckerVisitor checker;

        /// <summary>
        /// Checks the operations over mutable and immutable symbols to make sure the storage rules
        /// are not being infringed
        /// </summary>
        private MutabilityCheckerVisitor mutabilityChecker;


        public SemanticAnalysis()
        {
            this.typeInferrer = new TypeInferrer();
            this.symbolTable = new SymbolTable(typeInferrer);

            this.resolver = new SymbolResolverVisitor(this.symbolTable, this.typeInferrer);
            this.inferrer = new TypeInferrerVisitor(this.symbolTable, this.typeInferrer);
            this.checker = new TypeCheckerVisitor(this.symbolTable);
            this.mutabilityChecker = new MutabilityCheckerVisitor(this.symbolTable);

            NameGenerator.Instance.Reset();

            /*var intClass = new Class();

            this.SymbolTable.NewSymbol("int", intClass);
            this.SymbolTable.EnterClassScope("int");
            this.SymbolTable.NewSymbol("toStr", new Function(String.Instance));
            this.SymbolTable.LeaveScope();*/
        }        

        public SymbolTable Run(Node ast)
        {
            // First step is to resolve all the symbols in the program
            this.resolver.Visit(ast);

            // If there are unresolved types, throw an exception
            this.symbolTable.UpdateSymbolReferences();
            this.symbolTable.ThrowIfUnresolved();

            Console.WriteLine("================");
            Console.WriteLine("SYMBOL RESOLVING");
            Console.WriteLine("================");
            Console.WriteLine(this.symbolTable.ToDebugString());

            //return this.symbolTable;

            Console.WriteLine(this.typeInferrer.ToDebugString());

            // Make the type inference
            this.inferrer.Visit(ast);

            Console.WriteLine("=============");
            Console.WriteLine("TYPE INFERRER");
            Console.WriteLine("=============");
            Console.WriteLine(this.symbolTable.ToDebugString());            
            Console.WriteLine(this.typeInferrer.ToDebugString());

            // Check all the operations are valid
            /*this.checker.Visit(ast);

            // Ensure the mutability rules of variables and function calls
            this.mutabilityChecker.Visit(ast);*/

            return this.symbolTable;
        }
    }
}
