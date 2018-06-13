// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class UnaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstUnaryNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstUnaryNode unary)
        {
            return unary.Left.Visit(visitor);
        }
    }
}
