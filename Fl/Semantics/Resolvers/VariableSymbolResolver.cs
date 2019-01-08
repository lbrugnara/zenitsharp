// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class VariableSymbolResolver : INodeVisitor<SymbolResolverVisitor, VariableNode>
    {
        public void Visit(SymbolResolverVisitor visitor, VariableNode vardecl)
        {
            switch (vardecl)
            {
                case VariableDefinitionNode vardefnode:
                    VarDefinitionNode(visitor, vardefnode);
                    return;

                case VariableDestructuringNode vardestnode:
                    VarDestructuringNode(visitor, vardestnode);
                    return;
            }

            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected void VarDefinitionNode(SymbolResolverVisitor binder, VariableDefinitionNode vardecl)
        {
            // Get the type information:
            //  An anonymous type
            //  A named type
            var typeInfo = SymbolHelper.GetTypeInfo(binder.SymbolTable, binder.Inferrer, vardecl.Information.Type);
            
            foreach (var definition in vardecl.Definitions)
            {
                // Get the identifier name
                var variableName = definition.Left.Value;

                // Check if the symbol is already defined
                if (binder.SymbolTable.Contains(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                // Create the new symbol for the variable
                var symbol = binder.SymbolTable.Insert(variableName, typeInfo, Access.Public, SymbolHelper.GetStorage(vardecl.Information.Mutability));

                // If it is a variable definition, visit the right-hand side expression
                definition.Right?.Visit(binder);
            }
        }

        protected void VarDestructuringNode(SymbolResolverVisitor visitor, VariableDestructuringNode destrnode)
        {
            destrnode.Right.Visit(visitor);

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
                    : SymbolHelper.GetTypeInfo(visitor.SymbolTable, visitor.Inferrer, destrnode.Information.Type);

                // Create the new symbol for the variable
                var symbol = visitor.SymbolTable.Insert(variableName, varType, Access.Public, SymbolHelper.GetStorage(destrnode.Information.Mutability));
            }
        }
    }
}
