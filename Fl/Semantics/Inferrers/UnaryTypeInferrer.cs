// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class UnaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, UnaryNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, UnaryNode unary)
        {
            // TODO: Check Prefix/Postfix increment
            return unary.Left.Visit(visitor);
        }
    }
}
