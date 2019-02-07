// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Checkers
{
    class DeclarationTypeChecker : INodeVisitor<TypeCheckerVisitor, DeclarationNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, DeclarationNode decls)
        {
            foreach (Node statement in decls.Statements)
                statement.Visit(checker);

            return null;
        }
    }
}
