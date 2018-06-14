// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class AssignmentTypeChecker : INodeVisitor<TypeCheckerVisitor, AstAssignmentNode, SType>
    {
        public SType Visit(TypeCheckerVisitor checker, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, checker);

            throw new AstWalkerException($"Invalid variable assifnment of type {node.GetType().FullName}");
        }

        private SType MakeVariableAssignment(AstVariableAssignmentNode node, TypeCheckerVisitor checker)
        {
            SType leftHandSide = node.Accessor.Visit(checker);
            SType rightHandSide = node.Expression.Visit(checker);

            if (leftHandSide != rightHandSide)
                throw new System.Exception($"Cannot convert type {rightHandSide} to {leftHandSide}");

            return leftHandSide;
        }
    }
}
