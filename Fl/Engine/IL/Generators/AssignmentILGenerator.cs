// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Exceptions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Parser;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class AssignmentILGenerator : INodeVisitor<ILGenerator, AstAssignmentNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstAssignmentNode node)
        {
            if (node is AstVariableAssignmentNode)
                return MakeVariableAssignment(node as AstVariableAssignmentNode, generator);

            return null;
        }

        private Operand MakeVariableAssignment(AstVariableAssignmentNode node, ILGenerator generator)
        {
            SymbolOperand leftHandSide = node.Accessor.Exec(generator) as SymbolOperand;
            Operand rightHandSide = node.Expression.Exec(generator);

            if (node.AssignmentOp.Type == TokenType.Assignment)
            {
                generator.Emmit(new StoreInstruction(leftHandSide, rightHandSide));
                return leftHandSide;
            }


            Instruction instr = null;
            switch (node.AssignmentOp.Type)
            {
                case TokenType.IncrementAndAssign:
                    instr = new AddInstruction(leftHandSide, leftHandSide, rightHandSide);
                    break;
                case TokenType.DecrementAndAssign:
                    instr = new SubInstruction(leftHandSide, leftHandSide, rightHandSide);
                    break;
                case TokenType.MultAndAssign:
                    instr = new MultInstruction(leftHandSide, leftHandSide, rightHandSide);
                    break;
                case TokenType.DivideAndAssign:
                    instr = new DivInstruction(leftHandSide, leftHandSide, rightHandSide);
                    break;
                default:
                    throw new InvalidInstructionException($"Unsupported operation: {node.AssignmentOp.Value} ({node.AssignmentOp.Type})");
            }

            generator.Emmit(instr);
            return leftHandSide;
        }
    }
}
