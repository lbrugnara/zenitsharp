// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Exceptions;

namespace Fl.Semantics.Checkers
{
    class VariableTypeChecker : INodeVisitor<TypeCheckerVisitor, AstVariableNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstVariableNode vardecl)
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

        protected CheckedType VarDefinitionNode(TypeCheckerVisitor checker, AstVarDefinitionNode vardecl)
        {
            foreach (var declaration in vardecl.VarDefinitions)
            {
                // Get the variable type from the declaration
                var lhsType = checker.SymbolTable.GetSymbol(declaration.Item1.Value.ToString()).Type;

                // If it is a variable definition, get the right-hand side type info
                var rhs = declaration.Item2?.Visit(checker);

                if (rhs != null && !lhsType.IsAssignableFrom(rhs.Type))
                    throw new SymbolException($"Cannot assign type {rhs.Type} to variable of type {lhsType}");
            }

            return null;
        }

        protected CheckedType VarDestructuringNode(TypeCheckerVisitor checker, AstVarDestructuringNode vardestnode)
        {
            var initType = vardestnode.DestructInit.Visit(checker);

            for (int i = 0; i < vardestnode.Variables.Count; i++)
            {
                var declaration = vardestnode.Variables[i];

                // Get the variable type from the declaration
                var lhsType = checker.SymbolTable.GetSymbol(declaration.Value.ToString()).Type;
                var rhsType = (initType.Type as Tuple).Types[i];

                // When lhs is "var", take the type from the right hand side expression, or throw if it is not available
                if (!lhsType.IsAssignableFrom(rhsType))
                    throw new SymbolException($"Cannot assign type {rhsType} to variable of type {lhsType}");
            }

            return null;
        }
    }
}
