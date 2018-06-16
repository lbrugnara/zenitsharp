// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Symbols.Types;
using Fl.Symbols;

namespace Fl.TypeChecking.Checkers
{
    class BlockTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBlockNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstBlockNode node)
        {
            checker.SymbolTable.EnterScope(ScopeType.Common, $"block-{node.GetHashCode()}");
            foreach (AstNode statement in node.Statements)
            {
                statement.Visit(checker);
            }
            checker.SymbolTable.LeaveScope();
            return Null.Instance;
        }
    }
}
