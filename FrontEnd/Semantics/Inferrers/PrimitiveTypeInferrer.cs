// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.Primitives;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Inferrers
{
    class PrimitiveTypeInferrer : INodeVisitor<TypeInferrerVisitor, PrimitiveNode, IType>
    {
        public IType Visit(TypeInferrerVisitor inferrer, PrimitiveNode literal)
        {
            return new Primitive(SymbolHelper.GetType(inferrer.SymbolTable, literal.Literal), inferrer.SymbolTable.CurrentScope);
        }
    }
}
