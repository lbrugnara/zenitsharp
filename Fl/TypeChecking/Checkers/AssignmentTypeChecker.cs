// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class AssignmentTypeChecker : INodeVisitor<TypeCheckerVisitor, AstAssignmentNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, checker);

            throw new AstWalkerException($"Invalid variable assifnment of type {node.GetType().FullName}");
        }

        private CheckedType MakeVariableAssignment(AstVariableAssignmentNode node, TypeCheckerVisitor checker)
        {
            var leftHandSide = node.Accessor.Visit(checker);
            var rightHandSide = node.Expression.Visit(checker);

            if (!leftHandSide.Type.IsAssignableFrom(rightHandSide.Type))
                throw new System.Exception($"Cannot convert type {rightHandSide} to {leftHandSide}");

            leftHandSide.Symbol = null;

            return leftHandSide;
        }
    }
}
