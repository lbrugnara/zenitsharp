// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class AssignmentTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstAssignmentNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, visitor);

            throw new AstWalkerException($"Invalid variable assifnment of type {node.GetType().FullName}");
        }

        private Type MakeVariableAssignment(AstVariableAssignmentNode node, TypeInferrerVisitor visitor)
        {
            var leftHandSide = node.Accessor.Visit(visitor);
            var rightHandSide = node.Expression.Visit(visitor);

            return visitor.Inferrer.MakeConclusion(leftHandSide, rightHandSide);
        }
    }
}
