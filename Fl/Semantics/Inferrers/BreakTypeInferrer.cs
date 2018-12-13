// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Inferrers
{
    class BreakTypeInferrer : INodeVisitor<TypeInferrerVisitor, BreakNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, BreakNode wnode)
        {
            return wnode.Number?.Visit(visitor);
        }
    }
}
