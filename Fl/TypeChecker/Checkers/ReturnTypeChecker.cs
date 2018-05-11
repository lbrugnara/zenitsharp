// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Exceptions;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class ReturnTypeChecker : INodeVisitor<TypeChecker, AstReturnNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstReturnNode rnode)
        {
            if (!checker.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            return rnode.ReturnTuple?.Visit(checker);
        }
    }
}
