// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class BinarySymbolResolver : INodeVisitor<SymbolResolverVisitor, BinaryNode>
    {
        public void Visit(SymbolResolverVisitor visitor, BinaryNode binary)
        {
            binary.Left.Visit(visitor);
            binary.Right.Visit(visitor);
        }
    }
}
