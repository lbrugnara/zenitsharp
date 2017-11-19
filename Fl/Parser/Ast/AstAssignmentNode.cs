// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public class AstAssignmentNode : AstNode
    {
        public Token Identifier { get; }
        public Token AssignmentOp { get; }
        public AstNode Expression { get; }

        public AstAssignmentNode(Token identifier, Token assignmentOp, AstNode expression)
        {
            Identifier = identifier;
            AssignmentOp = assignmentOp;
            Expression = expression;
        }
    }
}
