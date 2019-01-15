// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class BinarySymbolResolver : INodeVisitor<SymbolResolverVisitor, BinaryNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, BinaryNode binary)
        {
            var left = binary.Left.Visit(visitor);
            var right = binary.Right.Visit(visitor);

            if (left is IBoundSymbol lbs && right is IBoundSymbol rbs)
                return visitor.Inferrer.FindMostGeneralType(lbs.TypeSymbol, rbs.TypeSymbol);

            if (left is ITypeSymbol lts && right is ITypeSymbol rts)
                return visitor.Inferrer.FindMostGeneralType(lts, rts);

            return null;
        }
    }
}
