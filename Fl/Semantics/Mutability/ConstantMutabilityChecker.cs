// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Exceptions;

namespace Fl.Semantics.Mutability
{
    class ConstantMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstConstantNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstConstantNode constdec)
        {
            foreach (var declaration in constdec.Constants)
                declaration.Item2.Visit(checker);

            return null;
        }
    }
}
