// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public class AstDestructuringAssignmentNode : AstAssignmentNode
    {
        public AstTupleNode Lvalues { get; }

        public AstDestructuringAssignmentNode(AstTupleNode lvalue, Token assignmentOp, AstNode expression)
            : base (assignmentOp, expression)
        {
            this.Lvalues = lvalue;
        }
    }
}
