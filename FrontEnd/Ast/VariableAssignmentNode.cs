// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class VariableAssignmentNode : AssignmentNode
    {
        public AccessorNode Accessor { get; }

        public VariableAssignmentNode(AccessorNode accessor, Token assignmentOp, Node expression)
            : base(assignmentOp, expression)
        {
            this.Accessor = accessor;
        }
    }
}
