﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public class AstVariableAssignmentNode : AstAssignmentNode
    {
        public AstAccessorNode Accessor { get; }

        public AstVariableAssignmentNode(AstAccessorNode accessor, Token assignmentOp, AstNode expression)
            : base(assignmentOp, expression)
        {
            Accessor = accessor;
        }
    }
}