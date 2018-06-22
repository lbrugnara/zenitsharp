﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Binders
{
    class WhileSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstWhileNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstWhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            visitor.SymbolTable.EnterScope(ScopeType.Loop, $"while-body-{wnode.GetHashCode()}");

            // Emmit the condition code
            wnode.Condition.Visit(visitor);

            // Emmit the body code
            wnode.Body.Visit(visitor);

            // Leave the while-block
            visitor.SymbolTable.LeaveScope();
        }
    }
}