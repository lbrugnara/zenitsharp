// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;
using System.Linq;

namespace Fl.Semantics.Checkers
{
    class ReturnTypeChecker : INodeVisitor<TypeCheckerVisitor, ReturnNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ReturnNode rnode)
        {
            if (!checker.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            // Get the current function's return symbol
            var returnSymbol = checker.SymbolTable.CurrentFunctionScope.ReturnSymbol;

            // If it is an empty return statement, leave 
            if (rnode.Expression == null)
            {
                if (returnSymbol.Type != Void.Instance)
                    throw new System.Exception($"Cannot return without an object of type '{returnSymbol.Type}'");

                return null;
            }

            // Visit the return's expression
            var checkedType = rnode.Expression.Visit(checker);

            // Get the return expression's type
            Object type = checkedType.Type;

            // The return statement expects a tuple and if that tuple contains
            // just one element, we use it as the return's type
            if ((type is Tuple t) && t.Types.Count == 1)
                type = t.Types.First();

            if (returnSymbol.Type == Void.Instance)
                throw new System.Exception($"Function '{checker.SymbolTable.CurrentFunctionScope.Uid}' returns void. Cannot return object of type '{type}'");

            if (!returnSymbol.Type.IsAssignableFrom(type))
                throw new System.Exception($"Function returns '{returnSymbol.Type}', cannot convert return value from '{type}'");

            return new CheckedType(type, checkedType.Symbol);
        }
    }
}
