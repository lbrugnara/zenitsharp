// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types.Specials;

namespace Fl.Semantics.Resolvers
{
    class BinarySymbolResolver : INodeVisitor<SymbolResolverVisitor, BinaryNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, BinaryNode binary)
        {
            var left = binary.Left.Visit(visitor);
            var right = binary.Right.Visit(visitor);

            var type = visitor.Inferrer.FindMostGeneralType(left.GetTypeSymbol(), right.GetTypeSymbol());

            if (type == null)
                type = new UnresolvedExpressionType("expr", visitor.SymbolTable.CurrentScope, left.GetTypeSymbol(), right.GetTypeSymbol());

            return type;
        }
    }
}
