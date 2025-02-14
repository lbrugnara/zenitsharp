﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class AssignmentTypeInferrer : INodeVisitor<TypeInferrerVisitor, AssignmentNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, AssignmentNode node)
        {
            if (node is VariableAssignmentNode)
                return this.MakeVariableAssignment(node as VariableAssignmentNode, visitor);
            if (node is DestructuringAssignmentNode)
                return this.MakeDestructuringAssignment(node as DestructuringAssignmentNode, visitor);

            throw new AstWalkerException($"Invalid variable assignment of type {node.GetType().FullName}");
        }

        private IType MakeVariableAssignment(VariableAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var leftHandSide = node.Accessor.Visit(visitor);
            var rightHandSide = node.Right.Visit(visitor);

            return visitor.Inferrer.FindMostGeneralType(leftHandSide, rightHandSide);
        }

        private IType MakeDestructuringAssignment(DestructuringAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var tupleIValueSymbol = node.Left.Visit(visitor);
            var exprIValueSymbol = node.Right.Visit(visitor);

            return visitor.Inferrer.FindMostGeneralType(tupleIValueSymbol, exprIValueSymbol);
        }
    }
}
