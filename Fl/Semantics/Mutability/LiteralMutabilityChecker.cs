// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics;

namespace Fl.Semantics.Mutability
{
    class LiteralMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstLiteralNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstLiteralNode literal)
        {
            return null;
        }
    }
}
