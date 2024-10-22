﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class DeclarationMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, DeclarationNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, DeclarationNode decls)
        {
            foreach (Node statement in decls.Declarations)
                statement.Visit(checker);

            return null;
        }
    }
}
