// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;
using Fl.Symbols;

namespace Fl.TypeChecking.Checkers
{
    class LiteralTypeChecker : INodeVisitor<TypeCheckerVisitor, AstLiteralNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstLiteralNode literal)
        {
            return new CheckedType(TypeHelper.FromToken(literal.Literal));
        }
    }
}
