﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Parser;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class BinaryILGenerator : INodeVisitor<ILGenerator, AstBinaryNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstBinaryNode binary)
        {
            Operand left = binary.Left.Exec(generator);
            Operand right = binary.Right.Exec(generator);

            SymbolOperand tmpname = generator.SymbolTable.NewTempSymbol();
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

            generator.Emmit(new VarInstruction(tmpname, null, null));
            generator.Emmit(instr);

            if (binary.Operator.Type == TokenType.NotEqual)
            {
                SymbolOperand notname = generator.SymbolTable.NewTempSymbol();
                generator.Emmit(new VarInstruction(notname, null, null));
                generator.Emmit(new NotInstruction(notname, tmpname));
                return notname;
            }

            return tmpname;
        }
    }
}
