// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types;
using System.Linq;

namespace Zenit.Semantics.Checkers
{
    class TupleTypeChecker : INodeVisitor<TypeCheckerVisitor, TupleNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, TupleNode node)
        {
            var types = node.Items?.Select(i => i?.Visit(checker)?.TypeSymbol);
            return new CheckedType(new TupleSymbol(checker.SymbolTable.CurrentScope, types.OfType<ITypeSymbol>().ToList()));
        }
    }
}
