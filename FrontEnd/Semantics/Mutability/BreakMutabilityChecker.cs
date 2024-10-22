﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class BreakMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, BreakNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, BreakNode wnode)
        {
            var nbreak = wnode.Number.Visit(checker);

            return nbreak;
        }
    }
}
