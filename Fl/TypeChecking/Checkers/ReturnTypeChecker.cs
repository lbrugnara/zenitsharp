// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols.Exceptions;
using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class ReturnTypeChecker : INodeVisitor<TypeCheckerVisitor, AstReturnNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstReturnNode rnode)
        {
            if (!checker.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            return rnode.ReturnTuple?.Visit(checker);
        }
    }
}
