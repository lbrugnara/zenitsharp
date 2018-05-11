// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Parser;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class AssignmentTypeChecker : INodeVisitor<TypeChecker, AstAssignmentNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, checker);

            return null;
        }

        private Symbol MakeVariableAssignment(AstVariableAssignmentNode node, TypeChecker checker)
        {
            Symbol leftHandSide = node.Accessor.Visit(checker);
            Symbol rightHandSide = node.Expression.Visit(checker);
            return leftHandSide;
        }
    }
}
