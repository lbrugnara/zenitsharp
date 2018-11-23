// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class AccessorTypeInferrer : INodeVisitor<TypeInferrerVisitor, AccessorNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, AccessorNode accessor)
        {
            Symbol symbol = null;
            string symbolName = accessor.Target.Value;

            // If this is the end of the accessor path, get the symbol in the current
            // scope and return its information
            if (accessor.Parent == null)
            {
                // Get accessed symbol that must be defined in the symtable's scope
                symbol = inferrer.SymbolTable.GetSymbol(symbolName);

                Struct type = symbol.Type;

                // Return the inferred type information for this symbol
                return new InferredType(type, symbol);
            }

            // If the accessed member has an eclosing accessor node, visit
            // it to get the enclosing symbol's type
            var encsym = accessor.Parent.Visit(inferrer).Symbol;

            // If the enclosing symbol implements ISymbolTable we will search for 
            // the symbol within the enclosing scope
            if (encsym is ISymbolTable)
            {
                symbol = (encsym as ISymbolTable).GetSymbol(symbolName);
                return new InferredType(symbol.Type, symbol);
            }

            // If the symbol is a class, we need to get the class's scope
            // to retrieve the class member
            if (encsym.Type is Class clasz)
            {
                // Find the Class scope
                symbol = inferrer.SymbolTable.GetClassScope(encsym.Name).GetSymbol(symbolName);
                return new InferredType(symbol.Type, symbol);
            }

            // Here we have to get the class's scope and the type must be one of the following types:
            //  - ClassInstance type
            //  - A native type
            ISymbolTable symtable = null;

            if (encsym.Type is ClassInstance classInstance)
                // Find the Class scope
                symtable = inferrer.SymbolTable.GetClassScope(classInstance.Class.ClassName);
            else if (inferrer.SymbolTable.TryGetSymbol(encsym.Type.Name)?.Type is Class)
                symtable = inferrer.SymbolTable.GetClassScope(encsym.Type.Name);
            else
                throw new SymbolException($"Unhandled accessor type {encsym}");

            symbol = symtable.GetSymbol(symbolName);

            // Either case, we are talking about an instance of a class or an instance of a primitive type, 
            // because of that we need to return the type of the member, and not just the ClassProperty or ClassMethod
            // type.
            return new InferredType(symbol.Type, symbol);
        }
    }
}
