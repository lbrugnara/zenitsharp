// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;
using Fl.Symbols.Exceptions;

namespace Fl.TypeChecking.Checkers
{
    class VariableTypeChecker : INodeVisitor<TypeCheckerVisitor, AstVariableNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstVariableNode vardecl)
        {
            switch (vardecl)
            {
                case AstVarDefinitionNode vardefnode:
                    return VarDefinitionNode(checker, vardefnode);

                case AstVarDestructuringNode vardestnode:
                    return VarDestructuringNode(checker, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected Type VarDefinitionNode(TypeCheckerVisitor checker, AstVarDefinitionNode vardecl)
        {
            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Get the variable type from the declaration
                var lhsType = checker.SymbolTable.GetSymbol(declaration.Item1.Value.ToString()).Type;

                // If it is a variable definition, get the right-hand side type info
                var rhsType = declaration.Item2?.Visit(checker);

                // When lhs is "var", take the type from the right hand side expression, or throw if it is not available
                if (lhsType == null)
                    lhsType = rhsType ?? throw new SymbolException("Implicitly-typed variable must be initialized");
                else if (rhsType != null && !lhsType.IsAssignableFrom(rhsType))
                    throw new SymbolException($"Cannot assign type {rhsType} to variable of type {lhsType}");
            }

            return null;
        }

        protected Type VarDestructuringNode(TypeCheckerVisitor checker, AstVarDestructuringNode vardestnode)
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
            return null;
        }
    }
}
