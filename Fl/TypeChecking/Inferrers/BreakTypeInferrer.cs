// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class BreakTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstBreakNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstBreakNode wnode)
        {
            var nbreak = wnode.Number.Visit(checker);

            return nbreak;
        }
    }
}
