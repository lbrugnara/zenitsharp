// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class AssignmentSymbolResolver : INodeVisitor<SymbolResolverVisitor, AssignmentNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(SymbolResolverVisitor visitor, AssignmentNode node)
        {
            if (node is VariableAssignmentNode)
                return this.MakeVariableAssignment(node as VariableAssignmentNode, visitor);

            if (node is DestructuringAssignmentNode)
                return this.MakeDestructuringAssignment(node as DestructuringAssignmentNode, visitor);

            throw new System.Exception($"Unhandled AssignmentNode type {node.GetType().FullName}");
        }

        private ITypeSymbol MakeVariableAssignment(VariableAssignmentNode node, SymbolResolverVisitor visitor)
        {
            var left = node.Accessor.Visit(visitor);
            node.Right.Visit(visitor);

            return left;
        }

        private ITypeSymbol MakeDestructuringAssignment(DestructuringAssignmentNode node, SymbolResolverVisitor visitor)
        {
            var left = node.Left.Visit(visitor);
            node.Right.Visit(visitor);

            return left;
        }
    }
}
