// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Checkers
{
    class AccessorTypeChecker : INodeVisitor<TypeCheckerVisitor, AstAccessorNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstAccessorNode accessor)
        {
            if (accessor.Enclosing != null)
                return accessor.Enclosing?.Visit(checker);

            return checker.SymbolTable.GetSymbol(accessor.Identifier.Value.ToString()).Type;
        }
    }
}
