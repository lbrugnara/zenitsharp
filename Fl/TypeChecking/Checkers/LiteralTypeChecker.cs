// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Lang.Types;
using Fl.Symbols;

namespace Fl.TypeChecking.Checkers
{
    class LiteralTypeChecker : INodeVisitor<TypeCheckerVisitor, AstLiteralNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstLiteralNode literal)
        {
            return TypeHelper.FromToken(literal.Literal);
        }
    }
}
