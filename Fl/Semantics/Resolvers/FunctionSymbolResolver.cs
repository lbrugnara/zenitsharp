// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class FunctionSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstFunctionNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstFunctionNode funcdecl)
        {
            // Create the function symbol
            var funcType = new Function();
            var funcSymbol = new Symbol(funcdecl.Name, funcType, Access.Public, Storage.Constant);

            // Register it in the current scope
            visitor.SymbolTable.AddSymbol(funcSymbol);

            // Change the current scope to be the function's scope
            visitor.SymbolTable.EnterScope(ScopeType.Function, funcdecl.Name);

            // Process the parameters
            funcdecl.Parameters.Parameters.ForEach(p => {
                // Define the symbol in the current scope (method's scope)
                var type = p.SymbolInfo.Type == null ? visitor.Inferrer.NewAnonymousType() : SymbolHelper.GetType(visitor.SymbolTable, p.SymbolInfo.Type);

                // Update the method's type
                funcType.DefineParameterType(type);

                var storage = SymbolHelper.GetStorage(p.SymbolInfo.Mutability);
                var symbol = new Symbol(p.Name.Value.ToString(), type, Access.Public, storage);

                if (visitor.Inferrer.IsTypeAssumption(type))
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
