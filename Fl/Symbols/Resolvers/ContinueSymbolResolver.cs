// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class ContinueSymbolResolver : INodeVisitor<SymbolResolver, AstContinueNode>
    {
        public void Visit(SymbolResolver checker, AstContinueNode cnode)
        {
        }
    }
}
