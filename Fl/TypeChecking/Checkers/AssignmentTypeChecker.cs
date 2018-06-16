// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class AssignmentTypeChecker : INodeVisitor<TypeCheckerVisitor, AstAssignmentNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, checker);

            throw new AstWalkerException($"Invalid variable assifnment of type {node.GetType().FullName}");
        }

        private Type MakeVariableAssignment(AstVariableAssignmentNode node, TypeCheckerVisitor checker)
        {
            Type leftHandSide = node.Accessor.Visit(checker);
            Type rightHandSide = node.Expression.Visit(checker);

            if (leftHandSide != rightHandSide)
                throw new System.Exception($"Cannot convert type {rightHandSide} to {leftHandSide}");

            return leftHandSide;
        }
    }
}
