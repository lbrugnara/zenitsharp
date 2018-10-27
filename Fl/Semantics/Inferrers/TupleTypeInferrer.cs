// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using System.Linq;

namespace Fl.Semantics.Inferrers
{
    class TupleTypeInferrer : INodeVisitor<TypeInferrerVisitor, TupleNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, TupleNode node)
        {
            var inferredTypes = node.Items?.Select(i => i?.Visit(visitor));
            return new InferredType(new Tuple(inferredTypes.Select(it => it?.Type).ToArray()));
        }
    }
}
