// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class AssignmentTypeInferrer : INodeVisitor<TypeInferrerVisitor, AssignmentNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, AssignmentNode node)
        {
            if (node is VariableAssignmentNode)
                return this.MakeVariableAssignment(node as VariableAssignmentNode, visitor);
            if (node is DestructuringAssignmentNode)
                return this.MakeDestructuringAssignment(node as DestructuringAssignmentNode, visitor);

            throw new AstWalkerException($"Invalid variable assignment of type {node.GetType().FullName}");
        }

        private ITypeSymbol MakeVariableAssignment(VariableAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var leftHandSide = node.Accessor.Visit(visitor);
            var rightHandSide = node.Right.Visit(visitor);

            return visitor.Inferrer.FindMostGeneralType(leftHandSide, rightHandSide);
        }

        private ITypeSymbol MakeDestructuringAssignment(DestructuringAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var tupleIValueSymbol = node.Left.Visit(visitor);
            var exprIValueSymbol = node.Right.Visit(visitor);

            return visitor.Inferrer.FindMostGeneralType(tupleIValueSymbol, exprIValueSymbol);
        }
    }
}
