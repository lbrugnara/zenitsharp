﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;
using System.Linq;

namespace Fl.Semantics.Mutability
{
    class ReturnMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, ReturnNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, ReturnNode rnode)
        {
            var result = rnode.Expression?.Visit(checker);
            return result != null ? new MutabilityCheckResult(result.Symbol) : null;
        }
    }
}