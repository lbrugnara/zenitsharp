// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class UnarySymbolResolver : INodeVisitor<SymbolResolver, AstUnaryNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstUnaryNode unary)
        {
            return unary.Left.Visit(checker);
        }
    }
}
