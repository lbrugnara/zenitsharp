// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class AssignmentSymbolResolver : INodeVisitor<SymbolResolverVisitor, AssignmentNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, AssignmentNode node)
        {
            if (node is VariableAssignmentNode)
                return this.MakeVariableAssignment(node as VariableAssignmentNode, visitor);

            if (node is DestructuringAssignmentNode)
                return this.MakeDestructuringAssignment(node as DestructuringAssignmentNode, visitor);

            throw new System.Exception($"Unhandled AssignmentNode type {node.GetType().FullName}");
        }

        private IValueSymbol MakeVariableAssignment(VariableAssignmentNode node, SymbolResolverVisitor visitor)
        {
            var left = node.Accessor.Visit(visitor);
            var right = node.Right.Visit(visitor);

            if (!left.IsAssignable())
                throw new SymbolException($"'{left.Name}' is a {left.GetType().Name} and cannot be used as a left-hand side in an assignment expression");

            if (!right.IsAssignable())
                throw new SymbolException($"'{right.Name}' is a {right.GetType().Name} and cannot be used as a right-hand side in an assignment expression");

            // On an assignment statement, we always return the left-hand side type
            return left;
        }

        private IValueSymbol MakeDestructuringAssignment(DestructuringAssignmentNode node, SymbolResolverVisitor visitor)
        {
            var left = node.Left.Visit(visitor);
            var right = node.Right.Visit(visitor);

            if (!left.IsAssignable())
                throw new SymbolException($"'{left.Name}' is a {left.GetType().Name} and cannot be used as a left-hand side in an assignment expression");

            if (!right.IsAssignable()) 
                throw new SymbolException($"'{right.Name}' is a {right.GetType().Name} and cannot be used as a right-hand side in an assignment expression");

            // On an assignment statement, we always return the left-hand side type
            return left;
        }
    }
}
