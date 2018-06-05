// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Checkers
{
    class UnaryTypeChecker : INodeVisitor<TypeCheckerVisitor, AstUnaryNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstUnaryNode unary)
        {
            return unary.Left.Visit(checker);
        }
    }
}
