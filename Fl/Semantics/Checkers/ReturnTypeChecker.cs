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

            var returnSymbol = (checker.SymbolTable.CurrentScope as FunctionScope).ReturnSymbol;

            var checkedType = rnode.Expression?.Visit(checker);

            if (checkedType == null)
                if (returnSymbol.Type == Void.Instance)
                    return new CheckedType(Void.Instance);
                else
                    throw new SymbolException($"Function does not return '{returnSymbol.Type}' in all paths");

            Type type = checkedType.Type;
            Symbol symbol = checkedType.Symbol;

            // Just one lement is like
            //  return 1;
            //  return 2;
            //  etc
            if ((type is Tuple t) && t.Types.Count == 1)
                type = t.Types.First();

            if (returnSymbol.Type == Void.Instance)
                throw new SymbolException($"Function does not return '{type}' in all paths");

            if (!returnSymbol.Type.IsAssignableFrom(type))
                throw new System.Exception($"Function returns '{returnSymbol.Type}', cannot convert return value from '{type}'");

            return new CheckedType(type, symbol);
        }
    }
}
