using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System;

namespace Fl.Semantics.Binders
{
    class ClassSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstClassNode>
    {
        public void Visit(SymbolBinderVisitor binder, AstClassNode node)
        {
            // Define the class in the global scope
            var classType = new Class();
            var classSymbol = binder.SymbolTable.Global.NewSymbol(node.Name.Value.ToString(), classType);

            binder.SymbolTable.EnterClassScope(classSymbol.Name);

            node.Properties.ForEach(propertyNode => {
                propertyNode.Visit(binder);
                var propertyName = propertyNode.Name.Value.ToString();
                classType.Properties[propertyName] = binder.SymbolTable.GetSymbol(propertyName).Type as ClassProperty ?? throw new System.Exception($"Property type is not {typeof(ClassProperty).FullName}");
            });

            node.Constants.ForEach(constantNode => {
                constantNode.Visit(binder);
                var constantName = constantNode.Name.Value.ToString();
                classType.Properties[constantName] = binder.SymbolTable.GetSymbol(constantName).Type as ClassProperty ?? throw new System.Exception($"Constant type is not {typeof(ClassProperty).FullName}");
            });

            node.Methods.ForEach(methodNode => {
                methodNode.Visit(binder);
                classType.Methods[methodNode.Name] = binder.SymbolTable.GetSymbol(methodNode.Name).Type as ClassMethod ?? throw new System.Exception($"Method type is not {typeof(ClassMethod).FullName}");
            });

            binder.SymbolTable.LeaveScope();
        }
    }
}
