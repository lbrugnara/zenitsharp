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

            InferredType inferredType = null;

            if (leftHandSide.Type is Anonymous && !(rightHandSide.Type is Anonymous))
            {
                inferredType = new InferredType(rightHandSide.Type);

                checker.Constraints.InferTypeAs(leftHandSide.Type as Anonymous, inferredType.Type);
            }
            else
            {
                inferredType = new InferredType(leftHandSide.Type);

                if (rightHandSide.Type is Anonymous)
                {
                    checker.Constraints.InferTypeAs(rightHandSide.Type as Anonymous, inferredType.Type);
                }
            }

            return inferredType;
        }
    }
}
