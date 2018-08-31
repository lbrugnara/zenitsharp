// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class ForMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstForNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            checker.SymbolTable.EnterScope(ScopeType.Loop, $"for-{fornode.GetHashCode()}");

            fornode.Init.Visit(checker);

            fornode.Condition.Visit(checker);

            fornode.Body.Visit(checker);

            fornode.Increment.Visit(checker);

            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
