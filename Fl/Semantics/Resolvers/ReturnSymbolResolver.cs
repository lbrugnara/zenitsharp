// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Exceptions;
using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class ReturnSymbolResolver : INodeVisitor<SymbolResolverVisitor, ReturnNode>
    {
        public void Visit(SymbolResolverVisitor visitor, ReturnNode rnode)
        {
            if (!visitor.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            // If it is an empty return statement, we leave here, no need to work with the @ret symbol
            if (rnode.Expression == null)
                return;

            // We create the @ret symbol in the current function's scope
            if (visitor.SymbolTable.CurrentFunctionScope.ReturnSymbol == null)
                visitor.SymbolTable.CurrentFunctionScope.CreateReturnSymbol();

            // We visit the return's expression
            rnode.Expression.Visit(visitor);
        }
    }
}
