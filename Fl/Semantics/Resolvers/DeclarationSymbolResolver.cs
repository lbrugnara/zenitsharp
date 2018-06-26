// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class DeclarationSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstDeclarationNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
                statement.Visit(visitor);
        }
    }
}
