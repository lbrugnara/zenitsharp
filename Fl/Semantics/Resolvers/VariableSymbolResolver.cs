// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class VariableSymbolResolver : INodeVisitor<SymbolResolverVisitor, VariableNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(SymbolResolverVisitor visitor, VariableNode vardecl)
        {
            // Variable definition are statements and not expressions, therefore they do not return
            // the ITypeSymbol received from the right-hand side
            switch (vardecl)
            {
                case VariableDefinitionNode vardefnode:
                    VarDefinitionNode(visitor, vardefnode);
                    break;

                case VariableDestructuringNode vardestnode:
                    VarDestructuringNode(visitor, vardestnode);
                    break;

                default:
                    throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
            }

            return null;
        }

        protected void VarDefinitionNode(SymbolResolverVisitor binder, VariableDefinitionNode vardecl)
        {
            foreach (var definition in vardecl.Definitions)
            {
                // Get the identifier name
                var variableName = definition.Left.Value;

                // Check if the symbol is already defined
                if (binder.SymbolTable.Contains(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                // If it is a variable definition, visit the right-hand side expression
                var rhsSymbol = definition.Right?.Visit(binder);

                IBoundSymbol lhsSymbol = null;

                if (rhsSymbol == null)
                {
                    // Create the new symbol for the variable
                    var typeInfo = SymbolHelper.GetTypeSymbol(binder.SymbolTable, binder.Inferrer, vardecl.Information.Type);
                    var storage = SymbolHelper.GetStorage(vardecl.Information.Mutability);
                    lhsSymbol = binder.SymbolTable.Insert(variableName, typeInfo, Access.Public, storage);
                }
                else
                {
                    // Remove the bound -complex- symbol
                    // binder.SymbolTable.Remove(rhsSymbol.Id);
                    // Bind it to the variable name
                    binder.SymbolTable.Insert(variableName, new BoundSymbol(variableName, rhsSymbol, Access.Public, SymbolHelper.GetStorage(vardecl.Information.Mutability), binder.SymbolTable.CurrentScope));
                }
            }
        }

        protected void VarDestructuringNode(SymbolResolverVisitor visitor, VariableDestructuringNode destrnode)
        {
            var types = destrnode.Right.Visit(visitor);

            foreach (var declaration in destrnode.Left)
            {
                if (declaration == null)
                    continue;

                // Get the identifier name
                var variableName = declaration.Value;

                // Check if the symbol is already defined
                if (visitor.SymbolTable.Contains(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                // If the type anotation is not specific (uses 'var'), we need to create an anonymous type
                // for every variable. If not, we just get the type information from the token
                var varType = destrnode.Information.Type.Type == Syntax.TokenType.Variable 
                    ? visitor.Inferrer.NewAnonymousType() 
                    : SymbolHelper.GetTypeSymbol(visitor.SymbolTable, visitor.Inferrer, destrnode.Information.Type);

                // Create the new symbol for the variable
                var symbol = visitor.SymbolTable.Insert(variableName, varType, Access.Public, SymbolHelper.GetStorage(destrnode.Information.Mutability));
            }
        }
    }
}
