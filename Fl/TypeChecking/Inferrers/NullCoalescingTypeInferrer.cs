// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class NullCoalescingTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstNullCoalescingNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstNullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(checker);
            var right = nullc.Right.Visit(checker);

            return left ?? right ?? null;
        }
    }
}
