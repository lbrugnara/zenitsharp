// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class AssignmentSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstAssignmentNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                MakeVariableAssignment(node as AstVariableAssignmentNode, visitor);

        }

        private void MakeVariableAssignment(AstVariableAssignmentNode node, SymbolResolverVisitor visitor)
        {
            node.Accessor.Visit(visitor);
            node.Expression.Visit(visitor);
        }
    }
}
