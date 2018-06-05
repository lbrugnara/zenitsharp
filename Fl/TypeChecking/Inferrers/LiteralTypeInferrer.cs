// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Lang.Types;
using Fl.Symbols;

namespace Fl.TypeChecking.Inferrers
{
    class LiteralTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstLiteralNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor checker, AstLiteralNode literal)
        {
            return new InferredType
            {
                Type = TypeHelper.FromToken(literal.Literal)
            };
        }
    }
}
