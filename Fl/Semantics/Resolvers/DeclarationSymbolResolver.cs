// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class DeclarationSymbolResolver : INodeVisitor<SymbolResolverVisitor, DeclarationNode>
    {
        public void Visit(SymbolResolverVisitor visitor, DeclarationNode decls)
        {
            foreach (Node statement in decls.Statements)
                statement.Visit(visitor);
        }
    }
}
