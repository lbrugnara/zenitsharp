// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class BinarySymbolResolver : INodeVisitor<SymbolResolver, AstBinaryNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstBinaryNode binary)
        {
            Symbol left = binary.Left.Visit(checker);
            Symbol right = binary.Right.Visit(checker);
            return left;
        }
    }
}
