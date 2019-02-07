// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class BinaryMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, BinaryNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, BinaryNode binary)
        {
            var left = binary.Left.Visit(checker);
            var right = binary.Right.Visit(checker);

            return left;
        }
    }
}
