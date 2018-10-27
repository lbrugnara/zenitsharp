// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class BreakTypeInferrer : INodeVisitor<TypeInferrerVisitor, BreakNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, BreakNode wnode)
        {
            var nbreak = wnode.Number?.Visit(visitor);
            return nbreak;
        }
    }
}
