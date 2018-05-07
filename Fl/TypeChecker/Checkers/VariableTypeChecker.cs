// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Parser.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecker.Checkers
{
    class VariableTypeChecker : INodeVisitor<TypeChecker, AstVariableNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstVariableNode vardecl)
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

        protected Symbol VarDefinitionNode(TypeChecker checker, AstVarDefinitionNode vardecl)
        {
            // Get the variable type from the declaration
            var lhsType = TypeHelper.FromToken(vardecl.VarType.TypeToken);

            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Get the identifier name
                var variableName = declaration.Item1.Value.ToString();

                // Symbol should already be defined
                if (!checker.SymbolTable.SymbolIsDefinedInBlock(variableName))
                    throw new SymbolException($"Symbol {variableName} is not defined.");                

                // If it is a variable definition, get the right-hand side type info
                var rhs = declaration.Item2?.Visit(checker);
                var rhsType = rhs?.Type;

                // When lhs is "var", take the type from the right hand side expression, or throw if it is not available
                if (lhsType == null)
                    lhsType = rhsType ?? throw new SymbolException("Implicitly-typed variable must be initialized");
                else if (rhsType != null && !lhsType.IsAssignableFrom(rhsType))
                    throw new SymbolException($"Cannot assign type {rhsType} to variable of type {lhsType}");
            }

            return null;
        }

        protected Symbol VarDestructuringNode(TypeChecker checker, AstVarDestructuringNode vardestnode)
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
