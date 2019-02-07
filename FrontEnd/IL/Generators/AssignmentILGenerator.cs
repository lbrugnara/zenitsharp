// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions;
using Zenit.IL.Instructions.Exceptions;
using Zenit.IL.Instructions.Operands;
using Zenit.Syntax;
using Zenit.Ast;

namespace Zenit.IL.Generators
{
    class AssignmentILGenerator : INodeVisitor<ILGenerator, AssignmentNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AssignmentNode node)
        {
            if (node is VariableAssignmentNode)
                return MakeVariableAssignment(node as VariableAssignmentNode, generator);

            return null;
        }

        private Operand MakeVariableAssignment(VariableAssignmentNode node, ILGenerator generator)
        {
            SymbolOperand leftHandSide = node.Accessor.Visit(generator) as SymbolOperand;
            Operand rightHandSide = node.Right.Visit(generator);

            // If right-hand side exists and has a type, do some type checking
            /*if (rightHandSide.Type != leftHandSide.Type)
                throw new AstWalkerException($"Cannot convert {rightHandSide.Type} to {leftHandSide.Type} ('{leftHandSide.Name}')");*/

            if (node.Operator.Type == TokenType.Assignment)
            {
                generator.Emmit(new StoreInstruction(leftHandSide, rightHandSide));
                return leftHandSide;
            }


            Instruction instr = null;
            switch (node.Operator.Type)
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
                    throw new InvalidInstructionException($"Unsupported operation: {node.Operator.Value} ({node.Operator.Type})");
            }

            generator.Emmit(instr);

            return leftHandSide;
        }
    }
}
