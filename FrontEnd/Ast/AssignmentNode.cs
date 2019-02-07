// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public abstract class AssignmentNode : Node
    {
        public Token Operator { get; }
        public Node Right { get; }

        public AssignmentNode(Token assignmentOp, Node expression)
        {
            this.Operator = assignmentOp;
            this.Right = expression;
        }
    }
}
