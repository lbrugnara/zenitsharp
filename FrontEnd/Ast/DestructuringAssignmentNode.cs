// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class DestructuringAssignmentNode : AssignmentNode
    {
        public TupleNode Left { get; }

        public DestructuringAssignmentNode(TupleNode lvalue, Token assignmentOp, TupleNode expression)
            : base (assignmentOp, expression ?? throw new System.ArgumentNullException(nameof(expression), $"Destructuring involves using tuples"))
        {
            this.Left = lvalue;
        }

        public new TupleNode Right => base.Right as TupleNode;
    }
}
