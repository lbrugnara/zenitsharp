// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstDestructuringAssignmentNode : AstAssignmentNode
    {
        public AstTupleNode Variables { get; }

        public AstDestructuringAssignmentNode(AstTupleNode lvalue, Token assignmentOp, AstNode expression)
            : base (assignmentOp, expression)
        {
            this.Variables = lvalue;
        }
    }
}
