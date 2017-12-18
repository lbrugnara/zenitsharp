// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;
using System;
using System.Linq;

namespace Fl.Engine.IL.Generators
{
    class BinaryILGenerator : INodeVisitor<AstILGenerator, AstBinaryNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstBinaryNode binary)
        {
            Operand left = binary.Left.Exec(generator);
            Operand right = binary.Right.Exec(generator);

            OpCode opcode = OpCode.Nop;
            switch (binary.Operator.Type)
            {
                case TokenType.Addition:
                    opcode = OpCode.Add;
                    break;
                case TokenType.Minus:
                    opcode = OpCode.Sub;
                    break;
                case TokenType.Multiplication:
                    opcode = OpCode.Mult;
                    break;
                case TokenType.Division:
                    opcode = OpCode.Div;
                    break;


                case TokenType.Or:
                    opcode = OpCode.Or;
                    break;

                case TokenType.And:
                    opcode = OpCode.And;
                    break;


                case TokenType.GreatThan:
                    opcode = OpCode.Cgt;
                    break;

                case TokenType.GreatThanEqual:
                    opcode = OpCode.Cgte;
                    break;

                case TokenType.LessThan:
                    opcode = OpCode.Clt;
                    break;

                case TokenType.LessThanEqual:
                    opcode = OpCode.Clte;
                    break;
                

                case TokenType.Equal:
                case TokenType.NotEqual: // Use NOT instruction
                    opcode = OpCode.Ceq;
                    break;
            }

            SymbolOperand tmpname = generator.GenerateTemporalName();
            generator.Emmit(new VarInstruction(tmpname, null, null));
            generator.Emmit(new BinaryInstruction(opcode, tmpname, left, right));

            if (binary.Operator.Type == TokenType.NotEqual)
            {
                SymbolOperand notname = generator.GenerateTemporalName();
                generator.Emmit(new VarInstruction(notname, null, null));
                generator.Emmit(new UnaryInstruction(OpCode.Not, notname, tmpname));
                return notname;
            }

            return tmpname;
        }
    }
}
