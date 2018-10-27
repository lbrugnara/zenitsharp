using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System;

namespace Fl.Semantics.Resolvers
{
    class ClassSymbolResolver : INodeVisitor<SymbolResolverVisitor, ClassNode>
    {
        public void Visit(SymbolResolverVisitor binder, ClassNode node)
        {
            if (!binder.SymbolTable.CurrentScope.IsGlobal && !binder.SymbolTable.CurrentScope.IsPackage)
                throw new SymbolException($"Cannot define a class within a {binder.SymbolTable.CurrentScope.Type.ToString().ToLower()}");

            // Define the class in the global scope
            var className = node.Name.Value;
            var classType = new Class(className);
            var classSymbol = binder.SymbolTable.NewClassSymbol(className, classType, Access.Public);

            binder.SymbolTable.EnterClassScope(classSymbol.Name);

            node.Properties.ForEach(propertyNode => {
                propertyNode.Visit(binder);
                var propertyName = propertyNode.Name.Value;
                classType.Properties[propertyName] = binder.SymbolTable.GetSymbol(propertyName).Type;
            });

            node.Constants.ForEach(constantNode => {
                constantNode.Visit(binder);
                var constantName = constantNode.Name.Value;
                classType.Properties[constantName] = binder.SymbolTable.GetSymbol(constantName).Type;
            });

            node.Methods.ForEach(methodNode => {
                methodNode.Visit(binder);
                var method = binder.SymbolTable.GetSymbol(methodNode.Name).Type as Method ?? throw new System.Exception($"Method type is not {typeof(Function).FullName}");
                method.SetDefiningClass(classType);
                classType.Methods[methodNode.Name] = method;
            });

            binder.SymbolTable.LeaveScope();
        }
    }
}
