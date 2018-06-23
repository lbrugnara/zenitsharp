// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using System.Linq;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Checkers
{
    class ClassMethodTypeChecker : INodeVisitor<TypeCheckerVisitor, AstClassMethodNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstClassMethodNode method)
        {
            var methodSymbol = checker.SymbolTable.GetSymbol(method.Name);

            checker.SymbolTable.EnterScope(ScopeType.Function, method.Name);

            method.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new CheckedType(methodSymbol.Type, methodSymbol);
        }
    }
}
