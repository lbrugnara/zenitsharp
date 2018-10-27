// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class ClassMethodSymbolResolver : INodeVisitor<SymbolResolverVisitor, ClassMethodNode>
    {
        public void Visit(SymbolResolverVisitor visitor, ClassMethodNode method)
        {
            // Get the access modifier, and the storage type for the method declaration
            var accessMod = SymbolHelper.GetAccess(method.SymbolInfo.Access);

            // Create the type and the symbol
            var methodType = new Method();
            var methodSymbol = new Symbol(method.Name, methodType, accessMod, Storage.Constant);

            // Register it in the current scope
            visitor.SymbolTable.AddSymbol(methodSymbol);

            // Change the current scope to be the method's scope
            visitor.SymbolTable.EnterScope(ScopeType.Function, method.Name);

            // Process the parameters
            method.Parameters.ForEach(p => {
                // Define the symbol in the current scope (method's scope)
                var type = p.SymbolInfo.Type == null ? visitor.Inferrer.NewAnonymousType() : SymbolHelper.GetType(visitor.SymbolTable, p.SymbolInfo.Type);
                
                // Update the method's type
                methodType.DefineParameterType(type);

                var storage = SymbolHelper.GetStorage(p.SymbolInfo.Mutability);
                var symbol = new Symbol(p.Name.Value, type, Access.Public, storage);

                if (visitor.Inferrer.IsTypeAssumption(type))
                    visitor.Inferrer.AssumeSymbolTypeAs(symbol, type);

                visitor.SymbolTable.AddSymbol(symbol);
            });

            // Visit the method's body
            method.Body.ForEach(s => s.Visit(visitor));

            // Get the return symbol
            var retsym = visitor.SymbolTable.GetSymbol("@ret");

            // Assume the method's return type
            var rettype = visitor.Inferrer.NewAnonymousType();

            // Update the method's type
            methodType.SetReturnType(rettype);

            // Update the @ret symbol
            retsym.Type = rettype;
            visitor.Inferrer.AssumeSymbolTypeAs(retsym, rettype);

            // At this point, the method's type is an assumed type, register
            // the method's symbol under that assumption
            visitor.Inferrer.AssumeSymbolTypeAs(methodSymbol, methodType);

            // Restore previous scope
            visitor.SymbolTable.LeaveScope();
        }
    }
}
