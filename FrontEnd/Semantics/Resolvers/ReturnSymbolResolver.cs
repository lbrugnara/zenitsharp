// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types.Specials;

namespace Zenit.Semantics.Resolvers
{
    class ReturnSymbolResolver : INodeVisitor<SymbolResolverVisitor, ReturnNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, ReturnNode rnode)
        {
            var func = visitor.SymbolTable.GetCurrentFunction();

            // If it is an empty return statement, we update
            // the function's return type to void
            if (rnode.Expression == null)
            {
                func.Return.ChangeType(new VoidSymbol());
                return func.Return.TypeSymbol;
            }

            // At this point we do have an expression to evaluate,
            // so we get the result and update the function's return type accordingly 
            var ret = rnode.Expression.Visit(visitor);

            func.Return.ChangeType(ret.GetTypeSymbol());

            return func.Return.TypeSymbol;
        }
    }
}
