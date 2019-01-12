// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    public class CallableSymbolResolver : INodeVisitor<SymbolResolverVisitor, CallableNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(SymbolResolverVisitor visitor, CallableNode node)
        {
            var target = node.Target.Visit(visitor);

            node.Arguments.Expressions.ForEach(e => e.Visit(visitor));

            if (target is FunctionSymbol fs)
                return fs?.Return.TypeSymbol;

            return null;
        }
    }
}
