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

            return left;
        }
    }
}
