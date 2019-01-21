using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class ClassSymbolResolver : INodeVisitor<SymbolResolverVisitor, ClassNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor binder, ClassNode node)
        {
            // By now we just allow class definition at global scope or package scope (no nested classes)
            /*if (!binder.SymbolTable.CurrentScope.IsGlobal && !binder.SymbolTable.CurrentScope.IsPackage)
                throw new SymbolException($"Cannot define a class within a {binder.SymbolTable.CurrentScope.Type.ToString().ToLower()}");

            // There are 3 components in the class definition:
            //  Class' type: It is the concrete type that we need to "build" while resolving the symbol. It contains structural information
            //  Class' symbol: Represents the binding of the class to the global/package scope
            //  Class' scope: The class' symbol contains information about the binding and the type, we also need an scope to keep the class' definition
            // The symbol and the type are related and saved in the global/package scope while the ClassScope is part of the symbol table (a nested scope)

            // Create the new class type
            var classType = new Class(node.Name.Value);

            // Create the class symbol
            // TODO: Access modifier
            var classSymbol = binder.SymbolTable.NewClassSymbol(node.Name.Value, classType, Access.Public);

            // Create the class scope
            binder.SymbolTable.EnterClassScope(classSymbol.Name);

            node.Properties.ForEach(propertyNode => {
                propertyNode.Visit(binder);
                var propertyName = propertyNode.Name.Value;
                classType.Properties[propertyName] = binder.SymbolTable.GetSymbol(propertyName).IValueSymbol.Type;
            });

            node.Constants.ForEach(constantNode => {
                constantNode.Visit(binder);
                var constantName = constantNode.Name.Value;
                classType.Properties[constantName] = binder.SymbolTable.GetSymbol(constantName).IValueSymbol.Type;
            });

            node.Methods.ForEach(methodNode => {
                methodNode.Visit(binder);
                var method = binder.SymbolTable.GetSymbol(methodNode.Name).IValueSymbol.Type as Function ?? throw new System.Exception($"Method type is not {typeof(Function).FullName}");
                //method.SetDefiningClass(classType);
                classType.Methods[methodNode.Name] = method;
            });

            binder.SymbolTable.LeaveScope();*/
            return null;
        }
    }
}
