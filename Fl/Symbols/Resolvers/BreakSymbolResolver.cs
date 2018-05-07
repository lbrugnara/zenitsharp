// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class BreakSymbolResolver : INodeVisitor<SymbolResolver, AstBreakNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstBreakNode wnode)
        {
            return wnode.Number?.Visit(checker);
        }
    }
}
