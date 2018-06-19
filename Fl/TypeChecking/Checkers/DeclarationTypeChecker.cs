// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class DeclarationTypeChecker : INodeVisitor<TypeCheckerVisitor, AstDeclarationNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
                statement.Visit(checker);

            return null;
        }
    }
}
