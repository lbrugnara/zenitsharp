// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class AccessorTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstAccessorNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstAccessorNode accessor)
        {
            ISymbolTable table = visitor.SymbolTable;

            if (accessor.Enclosing != null)
                table = accessor.Enclosing?.Visit(visitor) as ISymbolTable;

            // Get accessed symbol 
            var symbol = table.GetSymbol(accessor.Identifier.Value.ToString());

            // Return the inferred type information for this symbol
            return new InferredType(symbol.Type, symbol);
        }
    }
}
