// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Binders
{
    class ForSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstForNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            visitor.SymbolTable.EnterScope(ScopeType.Loop, $"for-{fornode.GetHashCode()}");

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
        }
    }
}
