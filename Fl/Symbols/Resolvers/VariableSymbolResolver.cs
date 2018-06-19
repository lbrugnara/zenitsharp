// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Exceptions;
using Fl.Symbols.Types;

namespace Fl.Symbols.Resolvers
{
    class VariableSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstVariableNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstVariableNode vardecl)
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

        protected void VarDefinitionNode(SymbolResolverVisitor visitor, AstVarDefinitionNode vardecl)
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

        protected void VarDestructuringNode(SymbolResolverVisitor checker, AstVarDestructuringNode vardestnode)
        {
            // Get the variable type
            //TypeResolver typeresolver = TypeResolver.GetTypeResolverFromToken(vardestnode.VarType.TypeToken);

            /*vardestnode.DestructInit.Exec()

            foreach (var declaration in vardestnode.VarDefinitions)
            {
                var identifierToken = declaration.Item1;
                var initializerInstr = declaration.Item2?.Exec(checker);

                checker.Emmit(new LocalVarInstruction(dataType, identifierToken.Value.ToString(), initializerInstr?.TargetName));
            }*/
        }
    }
}
