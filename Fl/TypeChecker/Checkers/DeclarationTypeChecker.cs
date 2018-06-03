// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecker.Checkers
{
    class DeclarationTypeChecker : INodeVisitor<TypeCheckerVisitor, AstDeclarationNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
                statement.Visit(checker);

            return Null.Instance;
        }
    }
}
