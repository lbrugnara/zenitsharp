// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Checkers
{
    class IfTypeChecker : INodeVisitor<TypeCheckerVisitor, IfNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, IfNode ifnode)
        {
            var conditionType = ifnode.Condition.Visit(checker);

            if (conditionType.TypeSymbol.BuiltinType != BuiltinType.Bool)
                throw new System.Exception($"For condition needs a {BuiltinType.Bool.GetName()} expression");

            // Add a new common block for the if's boyd
            checker.SymbolTable.EnterBlockScope($"{ifnode.Uid}");

            // Generate the if's body
            ifnode.Then?.Visit(checker);

            // Leave the if's then block
            checker.SymbolTable.LeaveScope();

            if (ifnode.Else != null)
            {
                // Add a block for the else's body and generate it, then leave the block
                checker.SymbolTable.EnterBlockScope($"{ifnode.Uid}");

                ifnode.Else.Visit(checker);

                checker.SymbolTable.LeaveScope();
            }

            return null;
        }
    }
}
