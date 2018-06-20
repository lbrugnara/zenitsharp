// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Exceptions;
using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class ReturnSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstReturnNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstReturnNode rnode)
        {
            if (!visitor.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            rnode.ReturnTuple?.Visit(visitor);
        }
    }
}
