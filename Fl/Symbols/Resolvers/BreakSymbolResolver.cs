// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class BreakSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstBreakNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstBreakNode wnode)
        {
            wnode.Number?.Visit(checker);
        }
    }
}
