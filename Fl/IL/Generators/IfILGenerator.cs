// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.Ast;

namespace Fl.IL.Generators
{
    class IfILGenerator : INodeVisitor<ILGenerator, IfNode, Operand>
    {
        public Operand Visit(ILGenerator generator, IfNode ifnode)
        {
            // Get a (non-resolved) label to skip the if
            var thenExitPoint = generator.ProgramBuilder.NewLabel();

            // Generate the condition and check the result, using exitPoint
            // as the destination if the condition is true
            var condition = ifnode.Condition.Visit(generator);            
            generator.Emmit(new IfFalseInstruction(condition, thenExitPoint));
            
            // Add a new common block for the if's boyd
            generator.EnterBlock(BlockType.Common);

            // Generate the if's body
            ifnode.Then?.Visit(generator);

            // Leave the if's then block
            generator.LeaveBlock();

            if (ifnode.Else == null)
            {
                // Add a label to be patched on the next generated instruction
                generator.BindLabel(thenExitPoint);
            }
            else
            {
                // The exitPoint for the if's then will be the entryPoint
                // for the if's else part
                Label elseEntryPoint = thenExitPoint;

                // We need to add a goto instruction to jump from inside 
                // the if's then body, that way we don't fall through the else part.
                // The goto destination address is not know yet, because it needs to be resolved
                // after generating the else body
                var @goto = new GotoInstruction(null);
                generator.Emmit(@goto);

                // Backpatch the elseEntryPoint (thenExitPoint) here
                generator.BindLabel(elseEntryPoint);

                // Generate the label for the (pending) goto instruction
                var elseExitPoint = generator.ProgramBuilder.NewLabel();
                @goto.SetDestination(elseExitPoint);

                // Add a block for the else's body and generate it, then leave the block
                generator.EnterBlock(BlockType.Common);
                ifnode.Else.Visit(generator);                
                generator.LeaveBlock();

                // Finally, backpatch the goto to jump from the then's body to avoid
                // fall through the else's body
                generator.BindLabel(elseExitPoint);
            }
            return null;
        }
    }
}
