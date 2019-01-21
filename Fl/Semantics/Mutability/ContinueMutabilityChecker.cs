// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Mutability
{
    class ContinueMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, ContinueNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, ContinueNode cnode)
        {
            return null;
        }
    }
}
