// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class AccessorTypeChecker : INodeVisitor<TypeCheckerVisitor, AstAccessorNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstAccessorNode accessor)
        {
            // By default use the SymbolTable
            ISymbolTable symtable = checker.SymbolTable;

            // If the accessed member has an eclosing accessor node, visit
            // it to get the enclosing symbol's type
            if (accessor.Enclosing != null)
            {
                var encsym = accessor.Enclosing?.Visit(checker).Symbol;

                // If the enclosing symbol implements ISymbolTable we simply use it
                if (encsym is ISymbolTable)
                {
                    symtable = encsym as ISymbolTable;
                }
                else if (encsym.Type is Class clasz)
                {
                    // Find the Class scope
                    symtable = checker.SymbolTable.Global.GetNestedScope(ScopeType.Class, encsym.Name);
                }
                else if (checker.SymbolTable.TryGetSymbol(encsym.Type.Name)?.Type is Class)
                {
                    symtable = checker.SymbolTable.Global.GetNestedScope(ScopeType.Class, encsym.Type.Name);
                }
            }

            // Get accessed symbol that must be defined in the symtable's scope
            var symbol = symtable.GetSymbol(accessor.Identifier.Value.ToString());

            // Return the type check information for this symbol
            return new CheckedType(symbol.Type, symbol);
        }
    }
}
