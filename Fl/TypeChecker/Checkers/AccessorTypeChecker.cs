// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Engine.Symbols.Types;
using Fl.Ast;
using Fl.Symbols;

namespace Fl.TypeChecker.Checkers
{
    class AccessorTypeChecker : INodeVisitor<TypeChecker, AstAccessorNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstAccessorNode accessor)
        {
            string currentIdentifier = accessor.Identifier.Value.ToString();

            // If current ID is the root member (or the only accessed one) return its symbol
            if (accessor.Enclosing == null)
                // If the symbol is not defined, create a new one that is not resolved yet (it could be a symbol that is not yet defined)
                return checker.SymbolTable.GetSymbol(currentIdentifier) ?? new Symbol(currentIdentifier, null);

            // If not, resolve its parrent...
            Symbol parent = accessor.Enclosing.Visit(checker);

            /*// ... add current ID as accessor's member of parent
            parent.AddMember(new Symbol(currentIdentifier));*/
            // TODO: Fix type checker
            return null;
        }
    }
}
