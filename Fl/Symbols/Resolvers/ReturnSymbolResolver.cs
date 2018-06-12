// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Symbols.Exceptions;
using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class ReturnSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstReturnNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstReturnNode rnode)
        {
            if (!checker.SymbolTable.CurrentBlock.IsFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            rnode.ReturnTuple?.Visit(checker);
        }
    }
}
