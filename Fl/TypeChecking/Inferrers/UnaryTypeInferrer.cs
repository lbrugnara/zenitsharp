// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class UnaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstUnaryNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstUnaryNode unary)
        {
            return unary.Left.Visit(visitor);
        }
    }
}
