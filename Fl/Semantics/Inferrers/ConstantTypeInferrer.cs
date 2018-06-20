// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ConstantTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstConstantNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstConstantNode constdec)
        {
            Type lhsType = null;

            foreach (var declaration in constdec.Constants)
            {
                if (lhsType == null)
                    lhsType = visitor.SymbolTable.GetSymbol(declaration.Item1.Value.ToString()).Type;

                // Get the right-hand side operand (a must for a constant)
                var rhs = declaration.Item2.Visit(visitor);

                visitor.Inferrer.MakeConclusion(lhsType, rhs.Type);
            }

            return new InferredType(lhsType);
        }
    }
}
