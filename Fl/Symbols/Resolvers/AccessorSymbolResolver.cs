// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class AccessorSymbolResolver : INodeVisitor<SymbolResolver, AstAccessorNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstAccessorNode accessor)
        {
            string currentIdentifier = accessor.Identifier.Value.ToString();

            // If current ID is the root member (or the only accessed one) return its symbol
            if (accessor.Enclosing == null)
                // If the symbol is not defined, create a new one that is not resolved yet (it could be a symbol that is not yet defined)
                return checker.SymbolTable.CurrentBlock.HasSymbol(currentIdentifier) ? checker.SymbolTable.GetSymbol(currentIdentifier) : new Symbol(currentIdentifier, null);

            // If not, resolve its parent...
            Symbol parent = accessor.Enclosing.Visit(checker);

            // ...if it is a package, find the currentIndentifier within it
            if (parent is Package pkg)
            {
                if (pkg.HasSymbol(currentIdentifier))
                    return pkg[currentIdentifier];
                // Unresolved
                return new Symbol(currentIdentifier, null, pkg.MangledName);
            }

            throw new SymbolException($"Symbol {currentIdentifier} is not defined in current scope");
        }
    }
}
