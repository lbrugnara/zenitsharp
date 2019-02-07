// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using System.Linq;

namespace Zenit.Semantics.Mutability
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
