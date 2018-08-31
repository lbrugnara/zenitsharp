// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstDestructuringAssignmentNode : AstAssignmentNode
    {
        public AstTupleNode Variables { get; }

        public AstDestructuringAssignmentNode(AstTupleNode lvalue, Token assignmentOp, AstTupleNode expression)
            : base (assignmentOp, expression ?? throw new System.ArgumentNullException(nameof(expression), $"Destructuring involves using tuples"))
        {
            this.Variables = lvalue;
        }

        public new AstTupleNode Expression => base.Expression as AstTupleNode;
    }
}
