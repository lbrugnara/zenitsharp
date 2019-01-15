// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class WhileSymbolResolver : INodeVisitor<SymbolResolverVisitor, WhileNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, WhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            visitor.SymbolTable.EnterLoopScope(wnode.Uid);

            // Emmit the condition code
            wnode.Condition.Visit(visitor);

            // Emmit the body code
            wnode.Body.Visit(visitor);

            // Leave the while-block
            visitor.SymbolTable.LeaveScope();

            return null;
        }
    }
}
