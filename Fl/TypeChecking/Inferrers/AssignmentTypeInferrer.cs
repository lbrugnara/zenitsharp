// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Parser;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class AssignmentTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstAssignmentNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, visitor);

            throw new AstWalkerException($"Invalid variable assifnment of type {node.GetType().FullName}");
        }

        private InferredType MakeVariableAssignment(AstVariableAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var leftHandSide = node.Accessor.Visit(visitor);
            var rightHandSide = node.Expression.Visit(visitor);

            return new InferredType(visitor.Inferrer.MakeConclusion(leftHandSide.Type, rightHandSide.Type));
        }
    }
}
