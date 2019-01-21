// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Mutability
{
    class ClassMethodMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, ClassMethodNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, ClassMethodNode method)
        {
            checker.SymbolTable.EnterFunctionScope(method.Name);

            method.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
