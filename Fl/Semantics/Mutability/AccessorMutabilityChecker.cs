// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class AccessorMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AccessorNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AccessorNode accessor)
        {
            var id = accessor.Target.Value;
            var enclosing = accessor.Parent?.Visit(checker);

            var symtable = (enclosing?.Symbol as ISymbolContainer) ?? checker.SymbolTable;

            return new MutabilityCheckResult(symtable.GetSymbol(id));
        }
    }
}
