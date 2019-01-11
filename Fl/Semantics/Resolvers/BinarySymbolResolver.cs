// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class BinarySymbolResolver : INodeVisitor<SymbolResolverVisitor, BinaryNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(SymbolResolverVisitor visitor, BinaryNode binary)
        {
            var left = binary.Left.Visit(visitor);
            var right = binary.Right.Visit(visitor);

            if (left != null && right != null && visitor.Inferrer.CanUnify(left, right))
                return visitor.Inferrer.FindMostGeneralType(left, right);

            return null;
        }
    }
}
