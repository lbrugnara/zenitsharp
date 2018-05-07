// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class DeclarationSymbolResolver : INodeVisitor<SymbolResolver, AstDeclarationNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
                statement.Visit(checker);

            return null;
        }
    }
}
