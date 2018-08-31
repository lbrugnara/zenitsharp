// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class NullCoalescingMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstNullCoalescingNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstNullCoalescingNode nullc)
        {
            var lefths = nullc.Left.Visit(checker);
            var righths = nullc.Right.Visit(checker);

            return new MutabilityCheckResult(lefths.Symbol ?? righths.Symbol);
        }
    }
}
