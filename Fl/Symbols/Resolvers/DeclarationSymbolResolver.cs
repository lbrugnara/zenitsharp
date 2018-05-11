// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class DeclarationSymbolResolver : INodeVisitor<SymbolResolver, AstDeclarationNode>
    {
        public void Visit(SymbolResolver checker, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
                statement.Visit(checker);
        }
    }
}
