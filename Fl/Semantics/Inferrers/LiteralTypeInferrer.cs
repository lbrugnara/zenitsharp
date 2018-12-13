// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class LiteralTypeInferrer : INodeVisitor<TypeInferrerVisitor, LiteralNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, LiteralNode literal)
        {
            return new InferredType(new TypeInfo(SymbolHelper.GetType(inferrer.SymbolTable, literal.Literal)));
        }
    }
}
