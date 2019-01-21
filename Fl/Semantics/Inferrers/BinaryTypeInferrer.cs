// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class BinaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, BinaryNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, BinaryNode binary)
        {
            var left = binary.Left.Visit(visitor);
            var right = binary.Right.Visit(visitor);

            return visitor.Inferrer.FindMostGeneralType(left, right);
        }
    }
}
