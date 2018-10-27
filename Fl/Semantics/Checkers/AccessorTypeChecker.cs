// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class AccessorTypeChecker : INodeVisitor<TypeCheckerVisitor, AccessorNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AccessorNode accessor)
        {
            Symbol symbol = null;
            string symbolName = accessor.Target.Value;

            // If this is the end of the accessor path, get the symbol in the current
            // scope and return its information
            if (accessor.Parent == null)
            {
                // Get accessed symbol that must be defined in the symtable's scope
                symbol = checker.SymbolTable.GetSymbol(symbolName);

                Type type = symbol.Type;

                // Return the type check information for this symbol
                return new CheckedType(type, symbol);
            }

            // If the accessed member has an eclosing accessor node, visit
            // it to get the enclosing symbol's type
            var encsym = accessor.Parent.Visit(checker).Symbol;

            // If the enclosing symbol implements ISymbolTable we will search for 
            // the symbol within the enclosing scope
            if (encsym is ISymbolTable)
            {
                symbol = (encsym as ISymbolTable).GetSymbol(symbolName);
                return new CheckedType(symbol.Type, symbol);
            }

            // If the symbol is a class, we need to get the class's scope
            // to retrieve the class member
            if (encsym.Type is Class clasz)
            {
                // Find the Class scope
                symbol = checker.SymbolTable.GetClassScope(encsym.Name).GetSymbol(symbolName);
                return new CheckedType(symbol.Type, symbol);
            }

            // Here we have to get the class's scope and the type must be one of the following types:
            //  - ClassInstance type
            //  - A native type
            ISymbolTable symtable = null;

            if (encsym.Type is ClassInstance classInstance)
                // Find the Class scope
                symtable = checker.SymbolTable.GetClassScope(classInstance.Class.ClassName);
            else if (checker.SymbolTable.TryGetSymbol(encsym.Type.Name)?.Type is Class)
                symtable = checker.SymbolTable.GetClassScope(encsym.Type.Name);
            else
                throw new SymbolException($"Unhandled accessor type {encsym}");

            symbol = symtable.GetSymbol(symbolName);


            // Either case, we are talking about an instance of a class or an instance of a primitive type, 
            // because of that we need to return the type of the member, and not just the ClassProperty or ClassMethod
            // type.
            return new CheckedType(symbol.Type, symbol);
        }
    }
}
