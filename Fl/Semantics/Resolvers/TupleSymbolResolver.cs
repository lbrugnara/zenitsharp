// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class TupleSymbolResolver : INodeVisitor<SymbolResolverVisitor, TupleNode>
    {
        public void Visit(SymbolResolverVisitor visitor, TupleNode node)
        {
            node.Items?.ForEach(item => item.Visit(visitor));
        }
    }
}
