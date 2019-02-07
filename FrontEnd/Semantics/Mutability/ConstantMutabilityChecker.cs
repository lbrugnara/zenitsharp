// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class ConstantMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, ConstantNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, ConstantNode constdec)
        {
            foreach (var definition in constdec.Definitions)
                definition.Right.Visit(checker);

            return null;
        }
    }
}
