// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class IfSymbolResolver : INodeVisitor<SymbolResolverVisitor, IfNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, IfNode ifnode)
        {
            // Generate the condition and check the result, using exitPoint
            // as the destination if the condition is true
            ifnode.Condition.Visit(visitor);            
            
            // Add a new common block for the if's boyd
            visitor.SymbolTable.EnterBlockScope(ifnode.Uid);

            // Generate the if's body
            ifnode.Then?.Visit(visitor);

            // Leave the if's then block
            visitor.SymbolTable.LeaveScope();

            if (ifnode.Else != null)
            {
                // Enter to the Else's scope
                visitor.SymbolTable.EnterBlockScope(ifnode.Uid);

                // Visit the else
                ifnode.Else.Visit(visitor);

                // Leave the else scope
                visitor.SymbolTable.LeaveScope();
            }

            return null;
        }
    }
}
