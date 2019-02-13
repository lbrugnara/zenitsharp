using Zenit.Ast;
using Zenit.Semantics.Resolvers;
using Zenit.Semantics.Checkers;
using Zenit.Semantics.Inferrers;
using Zenit.Semantics.Symbols;
using System;
using Zenit.Semantics.Mutability;
using Zenit.Helpers;

namespace Zenit.Semantics
{
    public class SemanticAnalysis
    {
        /// <summary>
        /// The type inferrer keeps track of unresolved types and granting anonymous types
        /// that will be replaced/updated once the types are resolved
        /// </summary>
        public TypeInferrer TypeInferrer { get; private set; }

        /// <summary>
        /// It keeps track of the scopes and symbols declared on each of them
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

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
            this.TypeInferrer = new TypeInferrer();
            this.SymbolTable = new SymbolTable(TypeInferrer);

            this.resolver = new SymbolResolverVisitor(this.SymbolTable, this.TypeInferrer);
            this.inferrer = new TypeInferrerVisitor(this.SymbolTable, this.TypeInferrer);
            this.checker = new TypeCheckerVisitor(this.SymbolTable);
            this.mutabilityChecker = new MutabilityCheckerVisitor(this.SymbolTable);

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
            this.SymbolTable.UpdateSymbolReferences();
            this.SymbolTable.ThrowIfUnresolved();

            Console.WriteLine("================");
            Console.WriteLine("SYMBOL RESOLVING");
            Console.WriteLine("================");
            Console.WriteLine(this.SymbolTable.ToDebugString());

            return this.SymbolTable;

            Console.WriteLine(this.TypeInferrer.ToDebugString());

            // Make the type inference
            this.inferrer.Visit(ast);

            Console.WriteLine("=============");
            Console.WriteLine("TYPE INFERRER");
            Console.WriteLine("=============");
            Console.WriteLine(this.SymbolTable.ToDebugString());            
            Console.WriteLine(this.TypeInferrer.ToDebugString());

            // Check all the operations are valid
            /*this.checker.Visit(ast);

            // Ensure the mutability rules of variables and function calls
            this.mutabilityChecker.Visit(ast);*/

            return this.SymbolTable;
        }
    }
}
