// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Inferrers
{
    class ConstantTypeInferrer : INodeVisitor<TypeInferrerVisitor, ConstantNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, ConstantNode constdec)
        {
            TypeInfo typeInfo = null;

            foreach (var definition in constdec.Definitions)
            {
                // Multiple constant definitions in the same statement are all of the same type so take the first
                if (typeInfo == null)
                    typeInfo = visitor.SymbolTable.GetSymbol(definition.Left.Value).TypeInfo;

                // Get the right-hand side expression-s type (a must for a constant)
                var rhs = definition.Right.Visit(visitor);

                visitor.Inferrer.Unify(typeInfo, rhs.TypeInfo);
            }

            return new InferredType(typeInfo);
        }
    }
}
