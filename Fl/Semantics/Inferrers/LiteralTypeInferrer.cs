// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class LiteralTypeInferrer : INodeVisitor<TypeInferrerVisitor, LiteralNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor inferrer, LiteralNode literal)
        {
            return new PrimitiveSymbol(SymbolHelper.GetType(inferrer.SymbolTable, literal.Literal), inferrer.SymbolTable.CurrentScope);
        }
    }
}
