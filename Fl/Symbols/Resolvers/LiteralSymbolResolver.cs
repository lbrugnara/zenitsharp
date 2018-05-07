// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class LiteralSymbolResolver : INodeVisitor<SymbolResolver, AstLiteralNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstLiteralNode literal)
        {
            return null;
        }
    }
}
