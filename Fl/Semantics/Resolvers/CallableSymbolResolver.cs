// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    public class CallableSymbolResolver : INodeVisitor<SymbolResolverVisitor, CallableNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, CallableNode node)
        {
            var target = node.Target.Visit(visitor);

            node.Arguments.Expressions.ForEach(e => e.Visit(visitor));

            if (target != null && target is IBoundSymbol bs && bs.TypeSymbol is FunctionSymbol fs)
                return fs?.Return.TypeSymbol;

            if (target != null && target is IValueSymbol && target is FunctionSymbol fs2)
                return fs2?.Return.TypeSymbol;

            if (target is UnresolvedTypeSymbol us)
                us.IsFunction = true;

            return target;
        }
    }
}
