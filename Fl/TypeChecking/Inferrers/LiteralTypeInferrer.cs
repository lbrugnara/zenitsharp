// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class LiteralTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstLiteralNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstLiteralNode literal)
        {
            return TypeHelper.FromToken(literal.Literal);
        }
    }
}
