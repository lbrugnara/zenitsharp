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
            var classType = new Class();

            // Define the class in the global scope
            var classSymbol = binder.SymbolTable.Global.NewSymbol(node.Name.Value.ToString(), classType);

            binder.SymbolTable.EnterClassScope(classSymbol.Name);

            node.Properties.ForEach(propertyNode => propertyNode.Visit(binder));

            node.Constants.ForEach(constantInfo => constantInfo.Visit(binder));

            node.Methods.ForEach(methodInfo => methodInfo.Visit(binder));

            binder.SymbolTable.LeaveScope();
        }
    }
}
