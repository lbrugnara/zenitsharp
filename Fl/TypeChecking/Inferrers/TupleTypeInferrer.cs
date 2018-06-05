// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Lang.Types;
using System.Linq;

namespace Fl.TypeChecking.Inferrers
{
    class TupleTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstTupleNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstTupleNode node)
        {
            var inferredTypes = node.Items?.Select(i => i.Visit(checker));
            // TODO: Handle tuple type
            return new InferredType
            {
                Type = new Tuple(inferredTypes.Select(it => it.Type).ToArray())
            };
        }
    }
}
