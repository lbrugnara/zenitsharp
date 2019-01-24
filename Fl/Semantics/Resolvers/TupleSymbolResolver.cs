// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Symbols.Types.Specials;
using System.Linq;

namespace Fl.Semantics.Resolvers
{
    class TupleSymbolResolver : INodeVisitor<SymbolResolverVisitor, TupleNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, TupleNode node)
        {
            var types = node.Items?.Select(item => item.Visit(visitor).GetTypeSymbol()).Cast<ITypeSymbol>().ToList();

            if (types.Any(t => t is IUnresolvedTypeSymbol))
                return new UnresolvedTupleType(visitor.SymbolTable.CurrentScope, types);

            // Tuples always return the TupleSymbol type
            var tupleType = new TupleSymbol(visitor.SymbolTable.CurrentScope, types);

            return tupleType;
        }
    }
}
