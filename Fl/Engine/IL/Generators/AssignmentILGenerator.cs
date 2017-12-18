// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.IL.Generators
{
    class AssignmentILGenerator : INodeVisitor<AstILGenerator, AstAssignmentNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, generator);

            return null;
        }

        private Operand MakeVariableAssignment(AstVariableAssignmentNode node, AstILGenerator generator)
        {
            SymbolOperand leftHandSide = node.Accessor.Exec(generator) as SymbolOperand;
            Operand rightHandSide = node.Expression.Exec(generator);

            if (node.AssignmentOp.Type == TokenType.Assignment)
            {
                generator.Emmit(new StoreInstruction(leftHandSide, rightHandSide));
                return leftHandSide;
            }


            OpCode opcode = OpCode.Nop;
            switch (node.AssignmentOp.Type)
            {
                case TokenType.IncrementAndAssign:
                    opcode = OpCode.Add;
                    break;
                case TokenType.DecrementAndAssign:
                    opcode = OpCode.Sub;
                    break;
                case TokenType.MultAndAssign:
                    opcode = OpCode.Mult;
                    break;
                case TokenType.DivideAndAssign:
                    opcode = OpCode.Div;
                    break;
            }

            generator.Emmit(new BinaryInstruction(opcode, leftHandSide, leftHandSide, rightHandSide));
            return leftHandSide;
        }
    }
}
