// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Checkers
{
    class BlockTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBlockNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstBlockNode node)
        {
            checker.SymbolTable.EnterScope(ScopeType.Common, $"block-{node.GetHashCode()}");

            foreach (AstNode statement in node.Statements)
                statement.Visit(checker);

            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
