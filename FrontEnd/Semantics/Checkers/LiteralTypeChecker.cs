// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Types;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Checkers
{
    class LiteralTypeChecker : INodeVisitor<TypeCheckerVisitor, LiteralNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, LiteralNode literal)
        {
            return new CheckedType(new PrimitiveSymbol(SymbolHelper.GetType(checker.SymbolTable, literal.Literal), checker.SymbolTable.CurrentScope));
        }
    }
}
