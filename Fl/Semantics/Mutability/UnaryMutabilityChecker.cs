// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class UnaryMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, UnaryNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, UnaryNode unary)
        {
            // TODO: Check Prefix/Postfix increment
            return unary.Left.Visit(checker);
        }
    }
}
