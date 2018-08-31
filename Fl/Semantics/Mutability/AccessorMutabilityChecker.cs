// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class AccessorMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstAccessorNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstAccessorNode accessor)
        {
            var id = accessor.Identifier.Value.ToString();
            var enclosing = accessor.Enclosing?.Visit(checker);

            var symtable = (enclosing?.Symbol as ISymbolTable) ?? checker.SymbolTable;

            return new MutabilityCheckResult(symtable.GetSymbol(id));
        }
    }
}
