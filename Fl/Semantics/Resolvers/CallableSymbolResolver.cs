// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types.Specials;

namespace Fl.Semantics.Resolvers
{
    public class CallableSymbolResolver : INodeVisitor<SymbolResolverVisitor, CallableNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, CallableNode node)
        {
            var target = node.Target.Visit(visitor);

            node.Arguments.Expressions.ForEach(e => e.Visit(visitor));

            var typeSymbol = target?.GetTypeSymbol();

            if (typeSymbol is FunctionSymbol fs)
                return fs?.Return.TypeSymbol;

            if (target is UnresolvedTypeSymbol us)
            {
                target = new UnresolvedFunctionSymbol(us.Name, us.Parent);
            }

            return target;
        }
    }
}
