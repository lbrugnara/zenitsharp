// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class ConstantTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstConstantNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstConstantNode constdec)
        {
            // Get the constant's type
            var lhsType = TypeHelper.FromToken(constdec.Type);

            foreach (var declaration in constdec.Constants)
            {
                // Get the right-hand side operand (a must for a constant)
                var rhs = declaration.Item2.Visit(visitor);

                visitor.Inferrer.MakeConclusion(lhsType, rhs.Type);
            }

            return new InferredType(lhsType);
        }
    }
}
