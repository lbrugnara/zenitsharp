// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class BreakTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstBreakNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstBreakNode wnode)
        {
            var nbreak = wnode.Number?.Visit(visitor);
            return nbreak;
        }
    }
}
