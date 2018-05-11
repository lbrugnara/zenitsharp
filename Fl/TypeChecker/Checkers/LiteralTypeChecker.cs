// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class LiteralTypeChecker : INodeVisitor<TypeChecker, AstLiteralNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstLiteralNode literal)
        {
            return null;
        }
    }
}
