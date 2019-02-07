// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Types;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Checkers
{
    class ReturnTypeChecker : INodeVisitor<TypeCheckerVisitor, ReturnNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ReturnNode rnode)
        {
            var func = checker.SymbolTable.GetCurrentFunction();

            // Get the current function's return symbol
            var returnSymbol = func.Return;

            // If it is an empty return statement, leave 
            if (rnode.Expression == null)
            {
                if (returnSymbol.TypeSymbol.BuiltinType != BuiltinType.Void)
                    throw new System.Exception($"Cannot return without an object of type '{returnSymbol.TypeSymbol}'");

                return null;
            }

            // Visit the return's expression
            var checkedType = rnode.Expression.Visit(checker);

            // Get the return expression's type
            var typeInfo = checkedType.TypeSymbol;

            // The return statement expects a tuple and if that tuple contains
            // just one element, we use it as the return's type
            /*if ((typeInfo.Type is Tuple t) && t.Types.Count == 1)
                typeInfo.ChangeType(t.Types.First());*/

            if (returnSymbol.TypeSymbol.BuiltinType == BuiltinType.Void)
                throw new System.Exception($"Function '{(checker.SymbolTable.CurrentScope as FunctionSymbol).Name}' returns void. Cannot return object of type '{typeInfo}'");

            /*if (!returnSymbol.TypeSymbol.Type.IsAssignableFrom(typeInfo.Type))
                throw new System.Exception($"Function returns '{returnSymbol.TypeSymbol}', cannot convert return value from '{typeInfo}'");*/

            return new CheckedType(typeInfo, checkedType.Symbol);
        }
    }
}
