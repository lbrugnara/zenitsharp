// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Mutability
{
    class AssignmentMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AssignmentNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AssignmentNode node)
        {
            if (node is VariableAssignmentNode)
                return CheckVariableAssignment(node as VariableAssignmentNode, checker);
            if (node is DestructuringAssignmentNode)
                return this.CheckDestructuringAssignment(node as DestructuringAssignmentNode, checker);

            throw new AstWalkerException($"Invalid variable assignment of type {node.GetType().FullName}");
        }

        private MutabilityCheckResult CheckVariableAssignment(VariableAssignmentNode node, MutabilityCheckerVisitor checker)
        {
            var leftHandSide = node.Accessor.Visit(checker);
            var rightHandSide = node.Right.Visit(checker);

            /*if (leftHandSide.Symbol.Storage == Symbols.Storage.Immutable)
                throw new System.Exception($"Cannot change value of immutable variable {leftHandSide.Symbol.Name} '{leftHandSide.Symbol.Name}'");*/

            return leftHandSide;
        }

        private MutabilityCheckResult CheckDestructuringAssignment(DestructuringAssignmentNode node, MutabilityCheckerVisitor checker)
        {
            for (int i = 0; i < node.Right.Items.Count; i++)
            {
                var varType = node.Right.Items[i];

                if (varType == null)
                    continue;

                var varnode = node.Left.Items[i];

                var leftHandSide = varnode.Visit(checker);

                /*if (leftHandSide.Symbol.Storage == Symbols.Storage.Immutable)
                    throw new System.Exception($"Cannot change value of immutable variable {leftHandSide.Symbol.Name} '{leftHandSide.Symbol.Name}'");*/
            }

            return null;
        }
    }
}
