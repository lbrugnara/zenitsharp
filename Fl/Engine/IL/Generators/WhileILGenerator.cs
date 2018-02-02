// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class WhileILGenerator : INodeVisitor<ILGenerator, AstWhileNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstWhileNode wnode)
        {
            // Generate the goto to re-test the condition (the destination is the next instruction: eblock)
            Label entryPoint = generator.Program.NewLabel();
            generator.BindLabel(entryPoint);

            // Generate a destination Label to leave the while-block (to be backpatched at the end)
            Label exitPoint = generator.Program.NewLabel();

            // Generate an eblock instruction for the whole while-block
            generator.EnterBlock(BlockType.Loop, entryPoint, exitPoint);

            // Emmit the condition code
            var condition = wnode.Condition.Exec(generator);

            // Emmit the if_false instruction with the break label
            generator.Emmit(new IfFalseInstruction(condition, exitPoint));

            // Emmit the body code
            wnode.Body.Exec(generator);

            // Leave the while-block
            generator.LeaveBlock();

            // Emmit the goto instruction to the start of the while-block
            generator.Emmit(new GotoInstruction(entryPoint));

            // Backpatch the exit label for the while-block
            generator.BindLabel(exitPoint);

            return null;
        }
    }
}
