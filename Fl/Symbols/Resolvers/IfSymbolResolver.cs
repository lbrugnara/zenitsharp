﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class IfSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstIfNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstIfNode ifnode)
        {
            // Generate the condition and check the result, using exitPoint
            // as the destination if the condition is true
            ifnode.Condition.Visit(visitor);            
            
            // Add a new common block for the if's boyd
            visitor.SymbolTable.EnterScope(ScopeType.Common, $"if-then-{ifnode.GetHashCode()}");

            // Generate the if's body
            ifnode.Then?.Visit(visitor);

            // Leave the if's then block
            visitor.SymbolTable.LeaveScope();

            if (ifnode.Else != null)
            {
                // Enter to the Else's scope
                visitor.SymbolTable.EnterScope(ScopeType.Common, $"if-else-{ifnode.GetHashCode()}");

                // Visit the else
                ifnode.Else.Visit(visitor);

                // Leave the else scope
                visitor.SymbolTable.LeaveScope();
            }
        }
    }
}
