// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using System.Linq;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Mutability
{
    class FunctionMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, FunctionNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, FunctionNode funcdecl)
        {
            checker.SymbolTable.EnterFunctionScope(funcdecl.Name);

            funcdecl.Body.ForEach(s => s.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new MutabilityCheckResult(checker.SymbolTable.GetSymbol(funcdecl.Name));
        }
    }
}
