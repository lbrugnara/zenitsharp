// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class ContinueSymbolResolver : INodeVisitor<SymbolResolver, AstContinueNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstContinueNode cnode)
        {
            return null;
        }
    }
}
