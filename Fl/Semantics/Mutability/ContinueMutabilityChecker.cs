// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class ContinueMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstContinueNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstContinueNode cnode)
        {
            return null;
        }
    }
}
