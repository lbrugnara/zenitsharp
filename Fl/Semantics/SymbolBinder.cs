// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Binders;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics
{
    public class SymbolBinder
    {
        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; }

        public TypeInferrer TypeInferrer { get; }

        public SymbolBinder()
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
            var visitor = new SymbolBinderVisitor(this.SymbolTable, this.TypeInferrer);
            visitor.Visit(node);
        }
    }
}
