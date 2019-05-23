// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Checkers
{
    class FunctionTypeChecker : INodeVisitor<TypeCheckerVisitor, FunctionNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, FunctionNode funcdecl)
        {
            // We just need to enter to the function's scope and run the type-checker through the
            // function's body
            var funcsym = checker.SymbolTable.GetVariableSymbol(funcdecl.Name);

            checker.SymbolTable.EnterFunctionScope(funcdecl.Name);

            funcdecl.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new CheckedType(funcsym.TypeSymbol, funcsym);
        }
    }
}
