// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class BreakTypeChecker : INodeVisitor<TypeChecker, AstBreakNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstBreakNode wnode)
        {
            return wnode.Number.Visit(checker);
        }
    }
}
