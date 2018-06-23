// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class AssignmentTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstAssignmentNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return this.MakeVariableAssignment(node as AstVariableAssignmentNode, visitor);
            if (node is AstDestructuringAssignmentNode)
                return this.MakeDestructuringAssignment(node as AstDestructuringAssignmentNode, visitor);

            throw new AstWalkerException($"Invalid variable assignment of type {node.GetType().FullName}");
        }

        private InferredType MakeVariableAssignment(AstVariableAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var leftHandSide = node.Accessor.Visit(visitor);
            var rightHandSide = node.Expression.Visit(visitor);

            // Make conclusions about the types if possible
            return new InferredType(visitor.Inferrer.MakeConclusion(leftHandSide.Type, rightHandSide.Type));
        }

        private InferredType MakeDestructuringAssignment(AstDestructuringAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var tupleInferredType = node.Variables.Visit(visitor);
            var exprInferredType = node.Expression.Visit(visitor);

            var tupleTypes = tupleInferredType.Type as Tuple;
            var exprTypes = exprInferredType.Type as Tuple;

            for (int i=0; i < tupleTypes.Count; i++)
            {
                var varType = tupleTypes.Types[i];

                if (varType == null)
                    continue;

                var exprType = exprTypes.Types[i];

                visitor.Inferrer.MakeConclusion(varType, exprType);
            }

            return exprInferredType;
        }
    }
}
