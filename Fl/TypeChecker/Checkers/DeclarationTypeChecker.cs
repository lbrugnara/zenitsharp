// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class DeclarationTypeChecker : INodeVisitor<TypeChecker, AstDeclarationNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
            {
                statement.Visit(checker);
            }
            return null;
        }
    }
}
