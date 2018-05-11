// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Engine.Symbols.Types;
using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class AccessorSymbolResolver : INodeVisitor<SymbolResolver, AstAccessorNode>
    {
        public void Visit(SymbolResolver checker, AstAccessorNode accessor)
        {
            accessor.Enclosing?.Visit(checker);
        }
    }
}
