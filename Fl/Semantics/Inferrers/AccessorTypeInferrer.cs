// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
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
            {
                var encsym = accessor.Enclosing?.Visit(visitor).Symbol;

                // If the enclosing symbol implements ISymbolTable we simply use it
                if (encsym is ISymbolTable)
                {                    
                    symtable = encsym as ISymbolTable;
                }
                else if (encsym.Type is Class || visitor.SymbolTable.TryGetSymbol(encsym.Type.Name)?.Type is Class)
                {
                    // Find the Class type
                    var clasz = encsym.Type as Class ?? visitor.SymbolTable.TryGetSymbol(encsym.Type.Name)?.Type as Class;

                    symtable = clasz.Methods.HasSymbol(accessor.Identifier.Value.ToString())
                            ? clasz.Methods
                            : clasz.Properties;
                }
            }

            // Get accessed symbol that must be defined in the symtable's scope
            var symbol = symtable.GetSymbol(accessor.Identifier.Value.ToString());

            // Return the inferred type information for this symbol
            return new InferredType(symbol.Type, symbol);
        }
    }
}
