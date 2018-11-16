// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class AssignmentTypeInferrer : INodeVisitor<TypeInferrerVisitor, AssignmentNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AssignmentNode node)
        {
            if (node is VariableAssignmentNode)
                return this.MakeVariableAssignment(node as VariableAssignmentNode, visitor);
            if (node is DestructuringAssignmentNode)
                return this.MakeDestructuringAssignment(node as DestructuringAssignmentNode, visitor);

            throw new AstWalkerException($"Invalid variable assignment of type {node.GetType().FullName}");
        }

        private InferredType MakeVariableAssignment(VariableAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var leftHandSide = node.Accessor.Visit(visitor);
            var rightHandSide = node.Right.Visit(visitor);

            // Make conclusions about the types if possible
            return new InferredType(visitor.Inferrer.InferFromType(leftHandSide.Type, rightHandSide.Type));
        }

        private InferredType MakeDestructuringAssignment(DestructuringAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var tupleInferredType = node.Left.Visit(visitor);
            var exprInferredType = node.Right.Visit(visitor);

            var tupleTypes = tupleInferredType.Type as Tuple;
            var exprTypes = exprInferredType.Type as Tuple;

            for (int i=0; i < tupleTypes.Count; i++)
            {
                var varType = tupleTypes.Types[i];

                if (varType == null)
                    continue;

                var exprType = exprTypes.Types[i];

                visitor.Inferrer.InferFromType(varType, exprType);
            }

            return exprInferredType;
        }
    }
}
