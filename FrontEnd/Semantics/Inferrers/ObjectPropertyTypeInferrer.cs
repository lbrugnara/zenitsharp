// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class ObjectPropertyTypeInferrer : INodeVisitor<TypeInferrerVisitor, ObjectPropertyNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, ObjectPropertyNode node)
        {
            var rightType = visitor.Visit(node.Value);

            var property = visitor.SymbolTable.GetVariableSymbol(node.Name.Value);

            // Get the most general type
            var generalType = visitor.Inferrer.FindMostGeneralType(property.TypeSymbol, rightType);
            
            // Unify the types
            visitor.Inferrer.Unify(visitor.SymbolTable, generalType, property);

            return property.TypeSymbol;
        }
    }
}
