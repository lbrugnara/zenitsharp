// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;
using System;

namespace Fl.Engine.IL.Generators
{
    class UnaryILGenerator : INodeVisitor<AstILGenerator, AstUnaryNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstUnaryNode unary)
        {
            // Generate the operand from the child
            var operand = unary.Left.Exec(generator);

            // If there is no unary operator, return operand to be handled in other ILGenerator
            if (unary.Operator == null)
                return operand;

            OpCode opcode;
            switch (unary.Operator.Type)
            {
                case TokenType.Not:
                    opcode = OpCode.Not;
                    break;
                case TokenType.Minus:
                    opcode = OpCode.Neg;
                    break;
                case TokenType.Increment:
                    if (!(operand is SymbolOperand))
                        throw new AstWalkerException($"The operand of an increment/decrement operator must be a variable");
                    opcode = unary is AstUnaryPostfixNode ? OpCode.PostInc : OpCode.PreInc;
                    break;
                case TokenType.Decrement:
                    if (!(operand is SymbolOperand))
                        throw new AstWalkerException($"The operand of an increment/decrement operator must be a variable");
                    opcode = unary is AstUnaryPostfixNode ? OpCode.PostDec : OpCode.PreDec;
                    break;
                default:
                    throw new AstWalkerException($"Unknown operator '{unary.Operator.Type}'");
            }

            // var @tX null
            // <unary> @tX <operand>
            var tmpsymbol = generator.GenerateTemporalName();
            generator.Emmit(new VarInstruction(tmpsymbol, null, null));
            generator.Emmit(new UnaryInstruction(opcode, tmpsymbol, operand));
            return tmpsymbol;
        }
    }
}
