// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class VariableMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, VariableNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, VariableNode vardecl)
        {
            switch (vardecl)
            {
                case VariableDefinitionNode vardefnode:
                    return VarDefinitionNode(checker, vardefnode);

                case VariableDestructuringNode vardestnode:
                    return VarDestructuringNode(checker, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected MutabilityCheckResult VarDefinitionNode(MutabilityCheckerVisitor checker, VariableDefinitionNode vardecl)
        {
            foreach (var definition in vardecl.Definitions)
                definition.Right?.Visit(checker);

            return null;
        }

        protected MutabilityCheckResult VarDestructuringNode(MutabilityCheckerVisitor checker, VariableDestructuringNode vardestnode)
        {
            vardestnode.Right.Visit(checker);

            return null;
        }
    }
}
