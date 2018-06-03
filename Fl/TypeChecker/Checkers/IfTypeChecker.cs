// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecker.Checkers
{
    class IfTypeChecker : INodeVisitor<TypeCheckerVisitor, AstIfNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstIfNode ifnode)
        {
            var conditionType = ifnode.Condition.Visit(checker);

            if (conditionType != Bool.Instance)
                throw new System.Exception($"For condition needs a {Bool.Instance} expression");

            // Add a new common block for the if's boyd
            checker.EnterBlock(BlockType.Common, $"if-then-{ifnode.GetHashCode()}");

            // Generate the if's body
            ifnode.Then?.Visit(checker);

            // Leave the if's then block
            checker.LeaveBlock();

            if (ifnode.Else != null)
            {
                // Add a block for the else's body and generate it, then leave the block
                checker.EnterBlock(BlockType.Common, $"if-else-{ifnode.GetHashCode()}");

                ifnode.Else.Visit(checker);

                checker.LeaveBlock();
            }

            return Null.Instance;
        }
    }
}
