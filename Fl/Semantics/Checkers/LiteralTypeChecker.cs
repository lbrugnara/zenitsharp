// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Checkers
{
    class LiteralTypeChecker : INodeVisitor<TypeCheckerVisitor, LiteralNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, LiteralNode literal)
        {
            return new CheckedType(new TypeInfo(SymbolHelper.GetType(checker.SymbolTable, literal.Literal)));
        }
    }
}
