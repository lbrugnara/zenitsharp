// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Exceptions;
using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class ReturnSymbolResolver : INodeVisitor<SymbolResolverVisitor, ReturnNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, ReturnNode rnode)
        {
            if (!visitor.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            var func = visitor.SymbolTable.GetCurrentFunctionScope();

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
