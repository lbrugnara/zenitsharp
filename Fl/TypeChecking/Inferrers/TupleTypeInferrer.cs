// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;
using System.Linq;

namespace Fl.TypeChecking.Inferrers
{
    class TupleTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstTupleNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstTupleNode node)
        {
            var inferredTypes = node.Items?.Select(i => i.Visit(visitor));
            // TODO: Handle tuple type
            return new Tuple(inferredTypes.Select(it => it.DataType).ToArray());
        }
    }
}
