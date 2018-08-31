// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    public class CallableMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstCallableNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstCallableNode node)
        {
            var target = node.Callable.Visit(checker);
            var funcScope = checker.SymbolTable.GetFunctionScope(target.Symbol.Name);

            node.Arguments.Expressions.ForEach(a => a.Visit(checker));

            return null;
        }
    }
}
