// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;
using System.Linq;

namespace Fl.Symbols.Resolvers
{
    class FuncDeclSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstFuncDeclNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstFuncDeclNode funcdecl)
        {
            // Create the function symbol
            var funcType = new Function();
            var funcSymbol = new Symbol(funcdecl.Name, funcType);

            // Register it in the current scope
            visitor.SymbolTable.AddSymbol(funcSymbol);

            // Change the current scope to be the function's scope
            visitor.SymbolTable.EnterScope(ScopeType.Function, funcdecl.Name);

            // Process the parameters
            funcdecl.Parameters.Parameters.ForEach(p => {

                // TODO: Type hinting needs to change this
                // By now, function's parameters are just declared
                // without type, assume anonymous types for all of them,
                // create their symbols and define them in the current scope 
                // (function's scope)
                var type = visitor.Inferrer.NewAnonymousType();

                // Update the function's type
                funcType.DefineParameterType(type);

                // Define the symbol in the current scope (function's scope)
                var symbol = new Symbol(p.Value.ToString(), type);
                visitor.Inferrer.AssumeSymbolTypeAs(symbol, type);
                visitor.SymbolTable.AddSymbol(symbol);
            });

            // Visit the function's body
            funcdecl.Body.ForEach(s => s.Visit(visitor));

            // Get the return symbol
            var retsym = visitor.SymbolTable.GetSymbol("@ret");

            // Assume the function's return type
            var rettype = visitor.Inferrer.NewAnonymousType();

            // Update the function's type
            funcType.SetReturnType(rettype);

            // Update the @ret symbol
            retsym.Type = rettype;
            visitor.Inferrer.AssumeSymbolTypeAs(retsym, rettype);            

            // At this point, the function's type is an assumed type, register
            // the function's symbol under that assumption
            visitor.Inferrer.AssumeSymbolTypeAs(funcSymbol, funcType);

            // Restore previous scope
            visitor.SymbolTable.LeaveScope();
        }
    }
}
