// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Resolvers
{
    class LiteralSymbolResolver : INodeVisitor<SymbolResolverVisitor, LiteralNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, LiteralNode literal)
        {
            return new PrimitiveSymbol(SymbolHelper.GetType(visitor.SymbolTable, literal.Literal), visitor.SymbolTable.CurrentScope);
        }
    }
}
