// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Exceptions;

namespace Fl.Semantics.Checkers
{
    class VariableTypeChecker : INodeVisitor<TypeCheckerVisitor, VariableNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, VariableNode vardecl)
        {
            switch (vardecl)
            {
                case VariableDefinitionNode vardefnode:
                    return VarDefinitionNode(checker, vardefnode);

                case VariableDestructuringNode vardestnode:
                    return VarDestructuringNode(checker, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected CheckedType VarDefinitionNode(TypeCheckerVisitor checker, VariableDefinitionNode vardecl)
        {
            foreach (var definition in vardecl.Definitions)
            {
                // Get the variable type from the declaration
                var lhsType = checker.SymbolTable.Lookup(definition.Left.Value).TypeInfo;

                // If it is a variable definition, get the right-hand side type info
                var rhs = definition.Right?.Visit(checker);

                if (rhs != null && !lhsType.Type.IsAssignableFrom(rhs.TypeInfo.Type))
                    throw new SymbolException($"Cannot assign type {rhs.TypeInfo} to variable of type {lhsType}");
            }

            return null;
        }

        protected CheckedType VarDestructuringNode(TypeCheckerVisitor checker, VariableDestructuringNode vardestnode)
        {
            var initType = vardestnode.Right.Visit(checker);

            for (int i = 0; i < vardestnode.Left.Count; i++)
            {
                var declaration = vardestnode.Left[i];

                if (declaration == null)
                    continue;

                // Get the variable type from the declaration
                var lhsType = checker.SymbolTable.Lookup(declaration.Value).TypeInfo;
                var rhsType = (initType.TypeInfo.Type as Tuple).Types[i];

                // When lhs is "var", take the type from the right hand side expression, or throw if it is not available
                if (!lhsType.Type.IsAssignableFrom(rhsType))
                    throw new SymbolException($"Cannot assign type {rhsType} to variable of type {lhsType}");
            }

            return null;
        }
    }
}
