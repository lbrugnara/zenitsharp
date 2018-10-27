// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class AssignmentSymbolResolver : INodeVisitor<SymbolResolverVisitor, AssignmentNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AssignmentNode node)
        {
            if (node is VariableAssignmentNode)
                this.MakeVariableAssignment(node as VariableAssignmentNode, visitor);
            else if (node is DestructuringAssignmentNode)
                this.MakeDestructuringAssignment(node as DestructuringAssignmentNode, visitor);
        }

        private void MakeVariableAssignment(VariableAssignmentNode node, SymbolResolverVisitor visitor)
        {
            node.Accessor.Visit(visitor);
            node.Right.Visit(visitor);
        }

        private void MakeDestructuringAssignment(DestructuringAssignmentNode node, SymbolResolverVisitor visitor)
        {
            node.Left.Visit(visitor);
            node.Right.Visit(visitor);
        }
    }
}
