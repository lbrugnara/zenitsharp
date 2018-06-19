// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class LiteralSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstLiteralNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstLiteralNode literal)
        {
        }
    }
}
