// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public class AstAssignmentNode : AstNode
    {
        public AstAccessorNode Accessor { get; }
        public AstTupleNode Lvalues { get; }
        public Token AssignmentOp { get; }        
        public AstNode Expression { get; }

        public AstAssignmentNode(AstAccessorNode accessor, Token assignmentOp, AstNode expression)
        {
            Accessor = accessor;
            AssignmentOp = assignmentOp;
            Expression = expression;
        }

        public AstAssignmentNode(AstTupleNode lvalue, Token assignmentOp, AstNode expression)
        {
            Lvalues = lvalue;
            AssignmentOp = assignmentOp;
            Expression = expression;
        }
    }
}
