// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using System.Linq;

namespace Fl.Semantics.Mutability
{
    class TupleMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, TupleNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, TupleNode node)
        {
            node.Items?.Select(i => i?.Visit(checker));
            return null;
        }
    }
}
