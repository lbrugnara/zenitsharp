// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Inferrers
{
    class AccessorTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstAccessorNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, AstAccessorNode accessor)
        {
            // TODO: Check this logic
            if (accessor.Enclosing != null)
                return accessor.Enclosing?.Visit(inferrer);

            // Get accessed symbol 
            var symbol = inferrer.SymbolTable.GetSymbol(accessor.Identifier.Value.ToString());

            // If it doesn't have a type, assign a temporal one
            if (symbol.Type == null)
                inferrer.Constraints.AssignTemporalType(symbol);

            // Return the inferred type information for this symbol
            return new InferredType(symbol.Type, symbol.Name);
        }
    }
}
