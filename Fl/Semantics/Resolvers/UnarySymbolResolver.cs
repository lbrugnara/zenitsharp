﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class UnarySymbolResolver : INodeVisitor<SymbolResolverVisitor, UnaryNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(SymbolResolverVisitor visitor, UnaryNode unary)
        {
            return unary.Left.Visit(visitor);
        }
    }
}
