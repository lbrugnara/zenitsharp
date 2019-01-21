// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types.Specials;

namespace Fl.Semantics.Resolvers
{
    class ReturnSymbolResolver : INodeVisitor<SymbolResolverVisitor, ReturnNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, ReturnNode rnode)
        {
            var func = visitor.SymbolTable.GetCurrentFunction();

            if (rnode.Expression == null)
            {
                func.Return.ChangeType(new VoidSymbol());
                return func.Return;
            }

            var ret = rnode.Expression.Visit(visitor);

            func.Return.ChangeType(ret.GetTypeSymbol());

            return func.Return;
        }
    }
}
