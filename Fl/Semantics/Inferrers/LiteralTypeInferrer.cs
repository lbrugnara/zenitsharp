// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class LiteralTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstLiteralNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstLiteralNode literal)
        {
            return new InferredType(TypeHelper.FromToken(literal.Literal));
        }
    }
}
