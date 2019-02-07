// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Resolvers
{
    class BlockSymbolResolver : INodeVisitor<SymbolResolverVisitor, BlockNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, BlockNode node)
        {
            visitor.SymbolTable.EnterBlockScope(node.Uid);

            foreach (Node statement in node.Statements)
                statement.Visit(visitor);

            visitor.SymbolTable.LeaveScope();

            return null;
        }
    }
}
