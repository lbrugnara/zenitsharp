// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class DeclarationSymbolResolver : INodeVisitor<SymbolResolverVisitor, DeclarationNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, DeclarationNode decls)
        {
            foreach (Node statement in decls.Statements)
                statement.Visit(visitor);

            return null;
        }
    }
}
