// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Binders
{
    class ClassMethodSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstClassMethodNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstClassMethodNode method)
        {
            // Get the access modifier, and the storage type for the method declaration
            var accessMod = SymbolHelper.GetAccessModifier(method.AccessModifier);

            // Create the type and the symbol
            var methodType = new ClassMethod(new Function(), accessMod, StorageType.Const);
            var methodSymbol = new Symbol(method.Name, methodType);

            // Register it in the current scope
            visitor.SymbolTable.AddSymbol(methodSymbol);

            // Change the current scope to be the method's scope
            visitor.SymbolTable.EnterScope(ScopeType.Function, method.Name);

            // Process the parameters
            method.Parameters.Parameters.ForEach(p => {

                // TODO: Type hinting needs to change this
                // By now, method's parameters are just declared
                // without type, assume anonymous types for all of them,
                // create their symbols and define them in the current scope 
                // (method's scope)
                var type = visitor.Inferrer.NewAnonymousType();

                // Update the method's type
                methodType.Type.DefineParameterType(type);

                // Define the symbol in the current scope (method's scope)
                var symbol = new Symbol(p.Value.ToString(), type);
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
            methodType.Type.SetReturnType(rettype);

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
