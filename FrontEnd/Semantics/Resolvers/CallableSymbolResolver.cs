// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types.References;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Types.Specials.Unresolved;

namespace Zenit.Semantics.Resolvers
{
    public class CallableSymbolResolver : INodeVisitor<SymbolResolverVisitor, CallableNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, CallableNode node)
        {
            var target = node.Target.Visit(visitor);

            node.Arguments.Expressions.ForEach(e => e.Visit(visitor));

            var typeSymbol = target?.GetTypeSymbol();

            // If the type symbol is a function, we return the @ret symbol's type
            if (typeSymbol is Function fs)
                return fs?.Return.TypeSymbol;

            // If target is an unresolved type, at this point we know it must be an unresolved function symbol
            if (target is UnresolvedSymbol us)
                target = new UnresolvedFunctionType(us.Name, us.Parent);

            return target;
        }
    }
}
