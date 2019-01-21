// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class ObjectPropertyTypeInferrer : INodeVisitor<TypeInferrerVisitor, ObjectPropertyNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, ObjectPropertyNode node)
        {
            var rightType = visitor.Visit(node.Value);

            var property = visitor.SymbolTable.GetBoundSymbol(node.Name.Value);

            // Get the most general type
            var generalType = visitor.Inferrer.FindMostGeneralType(property.TypeSymbol, rightType);
            
            // Unify the types
            visitor.Inferrer.Unify(visitor.SymbolTable, generalType, property);

            return property.TypeSymbol;
        }
    }
}
