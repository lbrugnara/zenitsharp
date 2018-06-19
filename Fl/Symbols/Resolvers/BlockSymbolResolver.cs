// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class BlockSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstBlockNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstBlockNode node)
        {
            visitor.SymbolTable.EnterScope(ScopeType.Common, $"block-{node.GetHashCode()}");

            foreach (AstNode statement in node.Statements)
                statement.Visit(visitor);

            visitor.SymbolTable.LeaveScope();
        }
    }
}
