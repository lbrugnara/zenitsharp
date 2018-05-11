// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class AssignmentSymbolResolver : INodeVisitor<SymbolResolver, AstAssignmentNode>
    {
        public void Visit(SymbolResolver checker, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                MakeVariableAssignment(node as AstVariableAssignmentNode, checker);

        }

        private void MakeVariableAssignment(AstVariableAssignmentNode node, SymbolResolver checker)
        {
            node.Accessor.Visit(checker);
            node.Expression.Visit(checker);
        }
    }
}
