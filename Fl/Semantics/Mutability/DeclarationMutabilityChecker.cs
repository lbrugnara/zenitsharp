// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Mutability
{
    class DeclarationMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, DeclarationNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, DeclarationNode decls)
        {
            foreach (Node statement in decls.Statements)
                statement.Visit(checker);

            return null;
        }
    }
}
