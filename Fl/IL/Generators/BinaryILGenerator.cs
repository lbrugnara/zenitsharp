// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;

namespace Fl.IL.Generators
{
    class BinaryILGenerator : INodeVisitor<ILGenerator, AstBinaryNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstBinaryNode binary)
        {
            Operand left = binary.Left.Visit(generator);
            Operand right = binary.Right.Visit(generator);

            SymbolOperand tmpname = generator.SymbolTable.NewTempSymbol(OperandType.Auto);

            Instruction instr = null;
            
            switch (binary.Operator.Type)
            {
                case TokenType.Addition:
                    instr = new AddInstruction(tmpname, left, right);
                    break;
                case TokenType.Minus:
                    instr = new SubInstruction(tmpname, left, right);
                    break;
                case TokenType.Multiplication:
                    instr = new MultInstruction(tmpname, left, right);
                    break;
                case TokenType.Division:
                    instr = new DivInstruction(tmpname, left, right);
                    break;


                case TokenType.Or:
                    instr = new OrInstruction(tmpname, left, right);
                    break;

                case TokenType.And:
                    instr = new AndInstruction(tmpname, left, right);
                    break;


                case TokenType.GreatThan:
                    instr = new CgtInstruction(tmpname, left, right);
                    break;

                case TokenType.GreatThanEqual:
                    instr = new CgteInstruction(tmpname, left, right);
                    break;

                case TokenType.LessThan:
                    instr = new CltInstruction(tmpname, left, right);
                    break;

                case TokenType.LessThanEqual:
                    instr = new ClteInstruction(tmpname, left, right);
                    break;


                case TokenType.Equal:
                case TokenType.NotEqual: // Use NOT instruction
                    instr = new CeqInstruction(tmpname, left, right);
                    break;
            }

            generator.Emmit(new VarInstruction(tmpname, null));
            generator.Emmit(instr);

            if (binary.Operator.Type == TokenType.NotEqual)
            {
                SymbolOperand notname = generator.SymbolTable.NewTempSymbol(OperandType.Auto);
                generator.Emmit(new VarInstruction(notname, null));
                generator.Emmit(new NotInstruction(notname, tmpname));
                return notname;
            }

            return tmpname;
        }
    }
}
