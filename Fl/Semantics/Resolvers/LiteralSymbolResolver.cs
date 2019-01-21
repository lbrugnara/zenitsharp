// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class LiteralSymbolResolver : INodeVisitor<SymbolResolverVisitor, LiteralNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, LiteralNode literal)
        {
            return new PrimitiveSymbol(SymbolHelper.GetType(visitor.SymbolTable, literal.Literal), visitor.SymbolTable.CurrentScope);
        }
    }
}
