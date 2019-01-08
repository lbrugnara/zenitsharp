// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using System.Linq;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Checkers
{
    class ClassMethodTypeChecker : INodeVisitor<TypeCheckerVisitor, ClassMethodNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ClassMethodNode method)
        {
            var methodSymbol = checker.SymbolTable.Lookup(method.Name);

            checker.SymbolTable.EnterFunctionScope(method.Name);

            method.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new CheckedType(methodSymbol.TypeInfo, methodSymbol);
        }
    }
}
