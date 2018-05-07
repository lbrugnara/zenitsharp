// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Parser;
using Fl.Parser.Ast;

namespace Fl.TypeChecker.Checkers
{
    class UnaryTypeChecker : INodeVisitor<TypeChecker, AstUnaryNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstUnaryNode unary)
        {
            return unary.Left.Visit(checker);
        }
    }
}
