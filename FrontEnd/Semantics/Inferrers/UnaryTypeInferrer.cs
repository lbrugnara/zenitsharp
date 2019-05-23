// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class UnaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, UnaryNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, UnaryNode unary)
        {
            // TODO: Check Prefix/Postfix increment
            return unary.Left.Visit(visitor);
        }
    }
}
