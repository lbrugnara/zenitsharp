// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class UnaryTypeChecker : INodeVisitor<TypeCheckerVisitor, AstUnaryNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstUnaryNode unary)
        {
            return unary.Left.Visit(checker);
        }
    }
}
