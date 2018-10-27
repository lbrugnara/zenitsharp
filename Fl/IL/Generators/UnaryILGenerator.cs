// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Syntax;
using Fl.Ast;

namespace Fl.IL.Generators
{
    class UnaryILGenerator : INodeVisitor<ILGenerator, UnaryNode, Operand>
    {
        public Operand Visit(ILGenerator generator, UnaryNode unary)
        {
            // Generate the operand from the child
            var operand = unary.Left.Visit(generator);

            // If there is no unary operator, return operand to be handled in other ILGenerator
            if (unary.Operator == null)
                return operand;

            var tmpsymbol = generator.SymbolTable.NewTempSymbol(OperandType.Auto);
            Instruction unaryInstruction = null;
            switch (unary.Operator.Type)
            {
                case TokenType.Not:
                    unaryInstruction = new NotInstruction(tmpsymbol, operand);
                    break;
                case TokenType.Minus:
                    unaryInstruction = new NegInstruction(tmpsymbol, operand);
                    break;
                case TokenType.Increment:
                    if (!(operand is SymbolOperand) || !(unary.Left is AccessorNode))
                        throw new AstWalkerException($"The operand of an increment/decrement operator must be a variable");

                    if (unary is UnaryPostfixNode)
                        unaryInstruction = new PostIncInstruction(tmpsymbol, operand as SymbolOperand);
                    else
                        unaryInstruction = new PreIncInstruction(tmpsymbol, operand as SymbolOperand);
                    break;
                case TokenType.Decrement:
                    if (!(operand is SymbolOperand) || !(unary.Left is AccessorNode))
                        throw new AstWalkerException($"The operand of an increment/decrement operator must be a variable");

                    if (unary is UnaryPostfixNode)
                        unaryInstruction = new PostDecInstruction(tmpsymbol, operand as SymbolOperand);
                    else
                        unaryInstruction = new PreDecInstruction(tmpsymbol, operand as SymbolOperand);
                    break;
                default:
                    throw new AstWalkerException($"Unknown operator '{unary.Operator.Type}'");
            }

            // var @tX null
            // <unary> @tX <operand>
            generator.Emmit(new VarInstruction(tmpsymbol, null));
            generator.Emmit(unaryInstruction);
            return tmpsymbol;
        }
    }
}
