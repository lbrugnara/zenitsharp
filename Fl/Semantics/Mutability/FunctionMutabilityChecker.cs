// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using System.Linq;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Mutability
{
    class FunctionMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstFunctionNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstFunctionNode funcdecl)
        {
            checker.SymbolTable.EnterScope(ScopeType.Function, funcdecl.Name);

            funcdecl.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new MutabilityCheckResult(checker.SymbolTable.GetSymbol(funcdecl.Name));
        }
    }
}
