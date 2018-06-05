// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class BinaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstBinaryNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstBinaryNode binary)
        {
            var left = binary.Left.Visit(checker);
            var right = binary.Right.Visit(checker);

            InferredType inferredType = null;

            if (left.Type is Anonymous && !(right.Type is Anonymous))
            {
                inferredType = new InferredType(right.Type);

                checker.Constraints.InferTypeAs(left.Type as Anonymous, inferredType.Type);
            }
            else
            {
                inferredType = new InferredType(left.Type);

                if (right.Type is Anonymous)
                {
                    checker.Constraints.InferTypeAs(right.Type as Anonymous, inferredType.Type);
                }
            }

            return inferredType;
        }
    }
}
