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
            var classScope = visitor.SymbolTable.CurrentScope as ClassScope;

            if (classScope == null)
                throw new SymbolException($"Current scope is not a class scope ({visitor.SymbolTable.CurrentScope.GetType().Name})");

            // Get the access modifier, and the storage type for the method declaration
            var accessMod = SymbolHelper.GetAccess(method.SymbolInfo.Access);

            // Create the type and the symbol
            var methodType = new Method();

            // Register it in the current scope
            var methodSymbol = classScope.CreateMethod(method.Name, methodType, accessMod);

            // Change the current scope to be the method's scope
            var methodScope = visitor.SymbolTable.EnterFunctionScope(method.Name);

            // Process the parameters
            method.Parameters.ForEach(p => {
                // Define the symbol in the current scope (method's scope)
                var type = p.SymbolInfo.Type == null 
                            ? visitor.Inferrer.NewAnonymousType() 
                            : SymbolHelper.GetType(visitor.SymbolTable, p.SymbolInfo.Type);
                
                // Update the method's type
                methodType.DefineParameterType(type);

                // Update function's scope with the parameter definition
                var storage = SymbolHelper.GetStorage(p.SymbolInfo.Mutability);
                var symbol = methodScope.CreateParameter(p.Name.Value, type, Access.Public, storage);

                // Update parameter-anonymous type if the type is an assumed type
                if (visitor.Inferrer.IsTypeAssumption(type))
                    visitor.Inferrer.AddTypeDependency(type, symbol);
            });

            // Visit the method's body
            method.Body.ForEach(s => s.Visit(visitor));

            // Assume the method's return type
            var rettype = visitor.Inferrer.NewAnonymousType();

            // Update the method's type
            methodType.SetReturnType(rettype);

            // Update the @ret symbol
            methodScope.ReturnSymbol.Type = rettype;
            visitor.Inferrer.AddTypeDependency(rettype, methodScope.ReturnSymbol);

            // At this point, the method's type is an assumed type, register
            // the method's symbol under that assumption
            visitor.Inferrer.AddTypeDependency(methodType, methodSymbol);

            // Restore previous scope
            visitor.SymbolTable.LeaveScope();
        }
    }
}
