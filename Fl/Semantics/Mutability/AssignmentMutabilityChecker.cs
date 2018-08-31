// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class AssignmentMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstAssignmentNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return CheckVariableAssignment(node as AstVariableAssignmentNode, checker);
            if (node is AstDestructuringAssignmentNode)
                return this.CheckDestructuringAssignment(node as AstDestructuringAssignmentNode, checker);

            throw new AstWalkerException($"Invalid variable assignment of type {node.GetType().FullName}");
        }

        private MutabilityCheckResult CheckVariableAssignment(AstVariableAssignmentNode node, MutabilityCheckerVisitor checker)
        {
            var leftHandSide = node.Accessor.Visit(checker);
            var rightHandSide = node.Expression.Visit(checker);

            /*if (leftHandSide.Symbol.Storage == Symbols.Storage.Immutable)
                throw new System.Exception($"Cannot change value of immutable variable {leftHandSide.Symbol.Name} '{leftHandSide.Symbol.Name}'");*/

            return leftHandSide;
        }

        private MutabilityCheckResult CheckDestructuringAssignment(AstDestructuringAssignmentNode node, MutabilityCheckerVisitor checker)
        {
            for (int i = 0; i < node.Expression.Items.Count; i++)
            {
                var varType = node.Expression.Items[i];

                if (varType == null)
                    continue;

                var varnode = node.Variables.Items[i];

                var leftHandSide = varnode.Visit(checker);

                /*if (leftHandSide.Symbol.Storage == Symbols.Storage.Immutable)
                    throw new System.Exception($"Cannot change value of immutable variable {leftHandSide.Symbol.Name} '{leftHandSide.Symbol.Name}'");*/
            }

            return null;
        }
    }
}
