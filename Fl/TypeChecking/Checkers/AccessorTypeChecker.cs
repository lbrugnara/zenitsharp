// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class AccessorTypeChecker : INodeVisitor<TypeCheckerVisitor, AstAccessorNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstAccessorNode accessor)
        {
            ISymbolTable symtable = checker.SymbolTable;

            if (accessor.Enclosing != null)
                symtable = accessor.Enclosing?.Visit(checker).Symbol as ISymbolTable;

            var symbol = symtable.GetSymbol(accessor.Identifier.Value.ToString());

            return new CheckedType(symbol.Type, symbol);
        }
    }
}
