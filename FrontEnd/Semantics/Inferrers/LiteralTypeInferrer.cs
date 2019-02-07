// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Inferrers
{
    class LiteralTypeInferrer : INodeVisitor<TypeInferrerVisitor, LiteralNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor inferrer, LiteralNode literal)
        {
            return new PrimitiveSymbol(SymbolHelper.GetType(inferrer.SymbolTable, literal.Literal), inferrer.SymbolTable.CurrentScope);
        }
    }
}
