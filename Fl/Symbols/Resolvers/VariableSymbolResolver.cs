// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.Symbols.Resolvers
{
    class VariableSymbolResolver : INodeVisitor<SymbolResolver, AstVariableNode>
    {
        public void Visit(SymbolResolver checker, AstVariableNode vardecl)
        {
            switch (vardecl)
            {
                case AstVarDefinitionNode vardefnode:
                    VarDefinitionNode(checker, vardefnode);
                    return;

                case AstVarDestructuringNode vardestnode:
                    VarDestructuringNode(checker, vardestnode);
                    return;
            }

            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected void VarDefinitionNode(SymbolResolver checker, AstVarDefinitionNode vardecl)
        {
            // Get the variable type from the declaration
            var lhsType = TypeHelper.FromToken(vardecl.VarType.TypeToken);

            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Get the identifier name
                var variableName = declaration.Item1.Value.ToString();

                // Check if the symbol is already defined
                if (checker.SymbolTable.HasSymbol(variableName))
                    throw new SymbolException($"Symbol {variableName} is already defined.");

                // Create the new symbol for the variable
                checker.SymbolTable.NewSymbol(variableName, lhsType);

                // If it is a variable definition, visit the right-hand side expression
                declaration.Item2?.Visit(checker);
            }
        }

        protected void VarDestructuringNode(SymbolResolver checker, AstVarDestructuringNode vardestnode)
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
