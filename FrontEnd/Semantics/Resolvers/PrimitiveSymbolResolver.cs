// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types.Primitives;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Resolvers
{
    class PrimitiveSymbolResolver : INodeVisitor<SymbolResolverVisitor, PrimitiveNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, PrimitiveNode literal)
        {
            return new Primitive(SymbolHelper.GetType(visitor.SymbolTable, literal.Literal), visitor.SymbolTable.CurrentScope);
        }
    }
}
