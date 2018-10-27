// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    public class CallableMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, CallableNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, CallableNode node)
        {
            var target = node.Target.Visit(checker);
            //var funcScope = checker.SymbolTable.GetFunctionScope(target.Symbol.Name);

            node.Arguments.Expressions.ForEach(a => a.Visit(checker));

            return null;
        }
    }
}
