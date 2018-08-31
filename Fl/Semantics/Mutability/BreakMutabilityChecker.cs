// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class BreakMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstBreakNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstBreakNode wnode)
        {
            var nbreak = wnode.Number.Visit(checker);

            return nbreak;
        }
    }
}
