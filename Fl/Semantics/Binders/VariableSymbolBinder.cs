// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;

namespace Fl.Semantics.Binders
{
    class VariableSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstVariableNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstVariableNode vardecl)
        {
            switch (vardecl)
            {
                case AstVarDefinitionNode vardefnode:
                    VarDefinitionNode(visitor, vardefnode);
                    return;

                case AstVarDestructuringNode vardestnode:
                    VarDestructuringNode(visitor, vardestnode);
                    return;
            }

            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected void VarDefinitionNode(SymbolBinderVisitor visitor, AstVarDefinitionNode vardecl)
        {
            // Get the variable type from the declaration or assume an anonymous type
            var lhsType = TypeHelper.FromToken(vardecl.VarType.TypeToken) ?? visitor.Inferrer.NewAnonymousType();

            var isAssumedType = visitor.Inferrer.IsTypeAssumption(lhsType);

            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Get the identifier name
                var variableName = declaration.Item1.Value.ToString();

                // Check if the symbol is already defined
                if (visitor.SymbolTable.HasSymbol(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                // Create the new symbol for the variable
                var symbol = visitor.SymbolTable.NewSymbol(variableName, lhsType);

                // If it is a type assumption, register the symbol under that assumption
                if (isAssumedType)
                    visitor.Inferrer.AssumeSymbolTypeAs(symbol, lhsType);

                // If it is a variable definition, visit the right-hand side expression
                declaration.Item2?.Visit(visitor);
            }
        }

        protected void VarDestructuringNode(SymbolBinderVisitor visitor, AstVarDestructuringNode destrnode)
        {
            destrnode.DestructInit.Visit(visitor);

            foreach (var declaration in destrnode.Variables)
            {
                // Get the identifier name
                var variableName = declaration.Value.ToString();

                // Check if the symbol is already defined
                if (visitor.SymbolTable.HasSymbol(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                var lhsType = visitor.Inferrer.NewAnonymousType();

                // Create the new symbol for the variable
                var symbol = visitor.SymbolTable.NewSymbol(variableName, lhsType);

                // Register the symbol under that assumption
                visitor.Inferrer.AssumeSymbolTypeAs(symbol, lhsType);
            }
        }
    }
}
