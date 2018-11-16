// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using System.Linq;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Checkers
{
    class FunctionTypeChecker : INodeVisitor<TypeCheckerVisitor, FunctionNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, FunctionNode funcdecl)
        {
            // We just need to enter to the function's scope and run the type-checker through the
            // function's body
            var funcsym = checker.SymbolTable.GetSymbol(funcdecl.Name);

            checker.SymbolTable.EnterScope(ScopeType.Function, funcdecl.Name);

            funcdecl.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new CheckedType(funcsym.Type, funcsym);
        }
    }
}
