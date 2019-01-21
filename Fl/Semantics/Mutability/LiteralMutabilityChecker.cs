// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Mutability
{
    class LiteralMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, LiteralNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, LiteralNode literal)
        {
            return null;
        }
    }
}
