// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Ast;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Resolvers
{
    class DeclarationSymbolResolver : INodeVisitor<SymbolResolverVisitor, DeclarationNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, DeclarationNode decls)
        {
            foreach (Node statement in decls.Declarations)
                statement.Visit(visitor);

            return null;
        }
    }
}
