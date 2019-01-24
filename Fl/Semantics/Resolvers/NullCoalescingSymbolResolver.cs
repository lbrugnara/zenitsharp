// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types.Specials;

namespace Fl.Semantics.Resolvers
{
    class NullCoalescingSymbolResolver : INodeVisitor<SymbolResolverVisitor, NullCoalescingNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, NullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(visitor);
            var right = nullc.Right.Visit(visitor);

            // Check if the types have a common ancestor
            var type = visitor.Inferrer.FindMostGeneralType(left.GetTypeSymbol(), right.GetTypeSymbol());

            // If "type" is null, the common ancestor cannot be evaluated, it could be because left or right are unresolved types,
            // so create a new unresolved expression type
            if (type == null)
                type = new UnresolvedExpressionType(visitor.SymbolTable.CurrentScope, left.GetTypeSymbol(), right.GetTypeSymbol());

            return type;
        }
    }
}
