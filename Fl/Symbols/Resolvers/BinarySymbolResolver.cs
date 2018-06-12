// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class BinarySymbolResolver : INodeVisitor<SymbolResolverVisitor, AstBinaryNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstBinaryNode binary)
        {
            binary.Left.Visit(checker);
            binary.Right.Visit(checker);
        }
    }
}
