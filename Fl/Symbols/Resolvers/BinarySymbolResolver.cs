// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class BinarySymbolResolver : INodeVisitor<SymbolResolver, AstBinaryNode>
    {
        public void Visit(SymbolResolver checker, AstBinaryNode binary)
        {
            binary.Left.Visit(checker);
            binary.Right.Visit(checker);
        }
    }
}
