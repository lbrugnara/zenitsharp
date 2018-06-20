// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class AssignmentSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstAssignmentNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                MakeVariableAssignment(node as AstVariableAssignmentNode, visitor);

        }

        private void MakeVariableAssignment(AstVariableAssignmentNode node, SymbolBinderVisitor visitor)
        {
            node.Accessor.Visit(visitor);
            node.Expression.Visit(visitor);
        }
    }
}
