﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class TupleSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstTupleNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstTupleNode node)
        {
            node.Items?.ForEach(item => item.Visit(checker));
        }
    }
}