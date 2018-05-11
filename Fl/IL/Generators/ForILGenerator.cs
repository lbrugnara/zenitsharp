// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.Ast;

namespace Fl.IL.Generators
{
    class ForILGenerator : INodeVisitor<ILGenerator, AstForNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            generator.EnterBlock(BlockType.Common);

            // Initialize the for-block
            fornode.Init.Visit(generator);

            // Generate the for's entry point label
            Label entryPoint = generator.ProgramBuilder.NewLabel();
            
            // Generate the for's exit point label (to be backpatched at the end)
            Label exitPoint = generator.ProgramBuilder.NewLabel();

            // Generate the loop block (for's body) with the entry and exit points
            generator.EnterBlock(BlockType.Loop, entryPoint, exitPoint);

            // The entry points starts here (before the for's condition), so bind the label
            generator.BindLabel(entryPoint);

            // Emmit the condition code
            var condition = fornode.Condition.Visit(generator);

            // Emmit the if_false instruction with the break label (if condition is false, go to exit point)
            generator.Emmit(new IfFalseInstruction(condition, exitPoint));

            // Emmit the body code
            fornode.Body.Visit(generator);

            // Emmit the for's increment part
            fornode.Increment.Visit(generator);

            // Leave the for
            generator.LeaveBlock();

            // Emmit the goto instruction to the for's condition (entry point)
            generator.Emmit(new GotoInstruction(entryPoint));

            // Backpatch the exit label for the for-block
            generator.BindLabel(exitPoint);

            // Leave the for-block
            generator.LeaveBlock();

            return null;
        }
    }
}
