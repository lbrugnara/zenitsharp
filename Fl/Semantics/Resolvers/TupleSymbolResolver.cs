// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;
using System.Linq;

namespace Fl.Semantics.Resolvers
{
    class TupleSymbolResolver : INodeVisitor<SymbolResolverVisitor, TupleNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, TupleNode node)
        {
            // Tuples always return the TupleSymbol type
            var types = new TupleSymbol("tuple", visitor.SymbolTable.CurrentScope)
            {
                Types = node.Items?.Select(item => item.Visit(visitor).GetTypeSymbol()).Cast<ITypeSymbol>().ToList()
            };

            return types != null && !types.Types.All(t => t == null) ? types : null;
        }
    }
}
