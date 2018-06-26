// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class AssignmentSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstAssignmentNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                this.MakeVariableAssignment(node as AstVariableAssignmentNode, visitor);
            else if (node is AstDestructuringAssignmentNode)
                this.MakeDestructuringAssignment(node as AstDestructuringAssignmentNode, visitor);
        }

        private void MakeVariableAssignment(AstVariableAssignmentNode node, SymbolResolverVisitor visitor)
        {
            node.Accessor.Visit(visitor);
            node.Expression.Visit(visitor);
        }

        private void MakeDestructuringAssignment(AstDestructuringAssignmentNode node, SymbolResolverVisitor visitor)
        {
            node.Variables.Visit(visitor);
            node.Expression.Visit(visitor);
        }
    }
}
