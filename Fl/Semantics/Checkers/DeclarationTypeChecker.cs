// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
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
