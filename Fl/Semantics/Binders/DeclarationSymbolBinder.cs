// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class DeclarationSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstDeclarationNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
                statement.Visit(visitor);
        }
    }
}
