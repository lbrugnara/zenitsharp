// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Checkers
{
    class UnaryTypeChecker : INodeVisitor<TypeCheckerVisitor, UnaryNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, UnaryNode unary)
        {
            // TODO: Check Prefix/Postfix increment
            return unary.Left.Visit(checker);
        }
    }
}
