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
        public InferredType Visit(TypeInferrerVisitor checker, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, checker);

            throw new AstWalkerException($"Invalid variable assifnment of type {node.GetType().FullName}");
        }

        private InferredType MakeVariableAssignment(AstVariableAssignmentNode node, TypeInferrerVisitor checker)
        {
            var leftHandSide = node.Accessor.Visit(checker);
            var rightHandSide = node.Expression.Visit(checker);

            return leftHandSide;
        }
    }
}
