// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class ContinueSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstContinueNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstContinueNode cnode)
        {
        }
    }
}
