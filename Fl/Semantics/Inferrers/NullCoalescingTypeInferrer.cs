// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class NullCoalescingTypeInferrer : INodeVisitor<TypeInferrerVisitor, NullCoalescingNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, NullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(visitor);
            var right = nullc.Right.Visit(visitor);

            return visitor.Inferrer.FindMostGeneralType(left, right);
        }
    }
}
