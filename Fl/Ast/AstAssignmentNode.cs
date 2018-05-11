// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser;

namespace Fl.Ast
{
    public abstract class AstAssignmentNode : AstNode
    {
        public Token AssignmentOp { get; }
        public AstNode Expression { get; }

        public AstAssignmentNode(Token assignmentOp, AstNode expression)
        {
            this.AssignmentOp = assignmentOp;
            this.Expression = expression;
        }
    }
}
