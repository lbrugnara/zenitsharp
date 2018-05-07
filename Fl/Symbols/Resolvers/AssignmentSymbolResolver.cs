// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class AssignmentSymbolResolver : INodeVisitor<SymbolResolver, AstAssignmentNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, checker);

            return null;
        }

        private Symbol MakeVariableAssignment(AstVariableAssignmentNode node, SymbolResolver checker)
        {
            Symbol leftHandSide = node.Accessor.Visit(checker);
            Symbol rightHandSide = node.Expression.Visit(checker);
            return leftHandSide;
        }
    }
}
