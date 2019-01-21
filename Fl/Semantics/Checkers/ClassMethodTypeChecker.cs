// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Checkers
{
    class ClassMethodTypeChecker : INodeVisitor<TypeCheckerVisitor, ClassMethodNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ClassMethodNode method)
        {
            var methodSymbol = checker.SymbolTable.GetBoundSymbol(method.Name);

            checker.SymbolTable.EnterFunctionScope(method.Name);

            method.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new CheckedType(methodSymbol.TypeSymbol, methodSymbol);
        }
    }
}
