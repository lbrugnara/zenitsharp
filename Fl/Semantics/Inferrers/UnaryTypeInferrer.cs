// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class UnaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstUnaryNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstUnaryNode unary)
        {
            return unary.Left.Visit(visitor);
        }
    }
}
