// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics;

namespace Fl.Semantics.Checkers
{
    class LiteralTypeChecker : INodeVisitor<TypeCheckerVisitor, AstLiteralNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstLiteralNode literal)
        {
            return new CheckedType(SymbolHelper.GetType(checker.SymbolTable, literal.Literal));
        }
    }
}
