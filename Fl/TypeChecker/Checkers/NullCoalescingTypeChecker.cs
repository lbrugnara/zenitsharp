// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class NullCoalescingTypeChecker : INodeVisitor<TypeChecker, AstNullCoalescingNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstNullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(checker);
            var right = nullc.Right.Visit(checker);

            return null;
        }
    }
}
