// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;
using Fl.Symbols.Resolvers;
using Fl.TypeChecking.Inferrers;

namespace Fl.Symbols
{
    public class SymbolResolver
    {
        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; }

        public TypeInferrer TypeInferrer { get; }

        public SymbolResolver()
        {
            this.SymbolTable = new SymbolTable();
            this.TypeInferrer = new TypeInferrer();

            /*Package std = new Package("std", this.SymbolTable.Scope.Uid);
            Package lang = std.NewPackage("lang");
            lang.NewSymbol("version", String.Instance);

            this.SymbolTable.AddSymbol(std);*/

            var intClass = new Class();
            intClass.Methods.NewSymbol("toStr", new Function(String.Instance));
            this.SymbolTable.NewSymbol("int", intClass);
        }

        public void Resolve(AstNode node)
        {
            var visitor = new SymbolResolverVisitor(this.SymbolTable, this.TypeInferrer);
            visitor.Visit(node);
        }
    }
}
