// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class UnarySymbolResolver : INodeVisitor<SymbolResolverVisitor, AstUnaryNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstUnaryNode unary)
        {
            unary.Left.Visit(checker);
        }
    }
}
