// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Exceptions;
using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class ReturnSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstReturnNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstReturnNode rnode)
        {
            if (!visitor.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            rnode.ReturnTuple?.Visit(visitor);
        }
    }
}
