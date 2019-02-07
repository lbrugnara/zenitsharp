// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Mutability
{
    class AccessorMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AccessorNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AccessorNode accessor)
        {
            var id = accessor.Target.Value;
            var enclosing = accessor.Parent?.Visit(checker);

            var symtable = (enclosing?.Symbol as ISymbolTable) ?? checker.SymbolTable;

            return new MutabilityCheckResult(symtable.GetBoundSymbol(id));
        }
    }
}
