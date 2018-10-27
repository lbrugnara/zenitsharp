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
            // Get the variable type from the declaration or assume an anonymous type
            var lhsType = SymbolHelper.GetType(binder.SymbolTable, binder.Inferrer, vardecl.Information.Type);

            var isAssumedType = binder.Inferrer.IsTypeAssumption(lhsType);

            foreach (var definition in vardecl.Definitions)
            {
                // Get the identifier name
                var variableName = definition.Left.Value;

                // Check if the symbol is already defined
                if (binder.SymbolTable.HasSymbol(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                // Create the new symbol for the variable
                var storage = SymbolHelper.GetStorage(vardecl.Information.Mutability);
                var symbol = binder.SymbolTable.NewSymbol(variableName, lhsType, Access.Public, storage);

                // If it is a type assumption, register the symbol under that assumption
                if (isAssumedType)
                    binder.Inferrer.AssumeSymbolTypeAs(symbol, lhsType);

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
                if (visitor.SymbolTable.HasSymbol(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                var lhsType = visitor.Inferrer.NewAnonymousType();

                // Create the new symbol for the variable
                var storage = SymbolHelper.GetStorage(destrnode.Information.Mutability);
                var symbol = visitor.SymbolTable.NewSymbol(variableName, lhsType, Access.Public, storage);

                // Register the symbol under that assumption
                visitor.Inferrer.AssumeSymbolTypeAs(symbol, lhsType);
            }
        }
    }
}
