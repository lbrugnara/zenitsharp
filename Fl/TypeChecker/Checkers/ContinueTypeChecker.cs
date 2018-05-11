// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class ContinueTypeChecker : INodeVisitor<TypeChecker, AstContinueNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstContinueNode cnode)
        {
            return null;
        }
    }
}
