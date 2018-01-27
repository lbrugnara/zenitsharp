// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class ForILGenerator : INodeVisitor<ILGenerator, AstForNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstForNode fornode)
        {
            // Generate a destination Label to leave the for-block (to be backpatched at the end)
            Label exitPoint = generator.Program.NewLabel();

            // Generate an eblock instruction for the for-block initialization
            generator.EnterBlock(BlockType.Common);

            // Initialize the for-block
            fornode.Init.Exec(generator);

            // Generate the goto to re-test the condition (the destination is the next instruction: eblock)
            Label entryPoint = generator.Program.NewLabel();
            generator.Labels.Push(entryPoint);

            // Generate an eblock instruction for the rest of the for-block
            generator.EnterBlock(BlockType.Loop, entryPoint, exitPoint);

            // Emmit the condition code
            var condition = fornode.Condition.Exec(generator);

            // Emmit the if_false instruction with the break label
            generator.Emmit(new IfFalseInstruction(condition, exitPoint));

            // Emmit the body code
            fornode.Body.Exec(generator);

            // Emmit the for-block's increment part
            fornode.Increment.Exec(generator);

            // Leave the for-block
            generator.LeaveBlock();

            // Emmit the goto instruction to the for-block's test part
            generator.Emmit(new GotoInstruction(entryPoint));

            // Backpatch the exit label for the for-block
            generator.Labels.Push(exitPoint);

            // Leave the for-block
            generator.LeaveBlock();

            return null;
        }
    }
}
