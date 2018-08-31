// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using System.Linq;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Mutability
{
    class ClassMethodMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstClassMethodNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstClassMethodNode method)
        {
            checker.SymbolTable.EnterScope(ScopeType.Function, method.Name);

            method.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
