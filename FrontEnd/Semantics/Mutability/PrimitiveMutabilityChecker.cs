// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class PrimitiveMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, PrimitiveNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, PrimitiveNode literal)
        {
            return null;
        }
    }
}
