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
            // By default use the SymbolTable
            ISymbolTable symtable = visitor.SymbolTable;

            // If the accessed member has an eclosing accessor node, visit
            // it to get the enclosing symbol's type
            if (accessor.Enclosing != null)
                symtable = accessor.Enclosing?.Visit(visitor) as ISymbolTable;

            // Get accessed symbol that must be defined in the symtable's scope
            var symbol = symtable.GetSymbol(accessor.Identifier.Value.ToString());

            // Return the inferred type information for this symbol
            return new InferredType(symbol.Type, symbol);
        }
    }
}
