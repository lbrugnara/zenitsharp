// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class BlockTypeInferrer : INodeVisitor<TypeInferrerVisitor, BlockNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, BlockNode node)
        {
            visitor.SymbolTable.EnterBlockScope(node.Uid);

            foreach (Node statement in node.Statements)
                statement.Visit(visitor);

            visitor.SymbolTable.LeaveScope();
            return null;
        }
    }
}
