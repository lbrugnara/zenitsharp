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
            var returnSymbol = (checker.SymbolTable.CurrentScope as FunctionSymbol).ReturnSymbol;

            // If it is an empty return statement, leave 
            if (rnode.Expression == null)
            {
                if (returnSymbol.TypeInfo.Type.BuiltinType != BuiltinType.Void)
                    throw new System.Exception($"Cannot return without an object of type '{returnSymbol.TypeInfo}'");

                return null;
            }

            // Visit the return's expression
            var checkedType = rnode.Expression.Visit(checker);

            // Get the return expression's type
            var typeInfo = checkedType.TypeInfo;

            // The return statement expects a tuple and if that tuple contains
            // just one element, we use it as the return's type
            if ((typeInfo.Type is Tuple t) && t.Types.Count == 1)
                typeInfo.ChangeType(t.Types.First());

            if (returnSymbol.TypeInfo.Type.BuiltinType == BuiltinType.Void)
                throw new System.Exception($"Function '{(checker.SymbolTable.CurrentScope as FunctionSymbol).Name}' returns void. Cannot return object of type '{typeInfo}'");

            if (!returnSymbol.TypeInfo.Type.IsAssignableFrom(typeInfo.Type))
                throw new System.Exception($"Function returns '{returnSymbol.TypeInfo}', cannot convert return value from '{typeInfo}'");

            return new CheckedType(typeInfo, checkedType.Symbol);
        }
    }
}
