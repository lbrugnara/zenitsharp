// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public abstract class AstAssignmentNode : AstNode
    {
        public Token AssignmentOp { get; }
        public AstNode Expression { get; }

        public AstAssignmentNode(Token assignmentOp, AstNode expression)
        {
            AssignmentOp = assignmentOp;
            Expression = expression;
        }
    }
}
