// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class BreakSymbolResolver : INodeVisitor<SymbolResolver, AstBreakNode>
    {
        public void Visit(SymbolResolver checker, AstBreakNode wnode)
        {
            wnode.Number?.Visit(checker);
        }
    }
}
