// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class ForSymbolResolver : INodeVisitor<SymbolResolverVisitor, ForNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, ForNode fornode)
        {
            // Create a new block to contain the for's initialization
            visitor.SymbolTable.EnterLoopScope(fornode.Uid);

            // Initialize the for-block
            fornode.Init.Visit(visitor);

            // Emmit the condition code
            fornode.Condition.Visit(visitor);

            // Emmit the body code
            fornode.Body.Visit(visitor);

            // Emmit the for's increment part
            fornode.Increment.Visit(visitor);

            // Leave the for
            visitor.SymbolTable.LeaveScope();

            return null;
        }
    }
}
