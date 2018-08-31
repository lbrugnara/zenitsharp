// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;
using System.Linq;

namespace Fl.Semantics.Mutability
{
    class ReturnMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstReturnNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstReturnNode rnode)
        {
            var result = rnode.ReturnTuple?.Visit(checker);
            return result != null ? new MutabilityCheckResult(result.Symbol) : null;
        }
    }
}
