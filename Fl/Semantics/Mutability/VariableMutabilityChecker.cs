// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Exceptions;

namespace Fl.Semantics.Mutability
{
    class VariableMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstVariableNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstVariableNode vardecl)
        {
            switch (vardecl)
            {
                case AstVarDefinitionNode vardefnode:
                    return VarDefinitionNode(checker, vardefnode);

                case AstVarDestructuringNode vardestnode:
                    return VarDestructuringNode(checker, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected MutabilityCheckResult VarDefinitionNode(MutabilityCheckerVisitor checker, AstVarDefinitionNode vardecl)
        {
            foreach (var declaration in vardecl.VarDefinitions)
                declaration.Item2?.Visit(checker);

            return null;
        }

        protected MutabilityCheckResult VarDestructuringNode(MutabilityCheckerVisitor checker, AstVarDestructuringNode vardestnode)
        {
            vardestnode.DestructInit.Visit(checker);

            return null;
        }
    }
}
