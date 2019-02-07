// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class FunctionMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, FunctionNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, FunctionNode funcdecl)
        {
            checker.SymbolTable.EnterFunctionScope(funcdecl.Name);

            funcdecl.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new MutabilityCheckResult(checker.SymbolTable.GetBoundSymbol(funcdecl.Name));
        }
    }
}
