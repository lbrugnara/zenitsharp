// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Types;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types.Primitives;

namespace Zenit.Semantics.Checkers
{
    class PrimitiveTypeChecker : INodeVisitor<TypeCheckerVisitor, PrimitiveNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, PrimitiveNode literal)
        {
            return new CheckedType(new Primitive(SymbolHelper.GetType(checker.SymbolTable, literal.Literal), checker.SymbolTable.CurrentScope));
        }
    }
}
