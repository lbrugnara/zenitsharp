// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class NullCoalescingILGenerator : INodeVisitor<ILGenerator, AstNullCoalescingNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstNullCoalescingNode nullc)
        {
            SymbolOperand retval = generator.SymbolTable.NewTempSymbol(OperandType.Auto, "retval");
            generator.Emmit(new VarInstruction(retval, null));

            // Get a (non-resolved) label to skip the if
            var leftExitPoint = generator.ProgramBuilder.NewLabel();

            // Generate the condition and check the result, using exitPoint
            // as the destination if the condition is true
            var left = nullc.Left.Exec(generator);
            SymbolOperand tmp = generator.SymbolTable.NewTempSymbol(OperandType.Auto, "check");
            generator.Emmit(new VarInstruction(tmp, null));
            generator.Emmit(new CeqInstruction(tmp, left, new ImmediateOperand(OperandType.Null, null)));
            generator.Emmit(new NotInstruction(tmp, tmp));
            generator.Emmit(new IfFalseInstruction(tmp, leftExitPoint));

            generator.Emmit(new StoreInstruction(retval, left));

            // We need to add a goto instruction to jump from inside 
            // the if's then body, that way we don't fall through the else part.
            // The goto destination address is not know yet, because it needs to be resolved
            // after generating the else body
            var @goto = new GotoInstruction(null);
            generator.Emmit(@goto);

            // The exitPoint for the if's then will be the entryPoint
            // for the if's else part
            Label elseEntryPoint = leftExitPoint;

            // Backpatch the elseEntryPoint (thenExitPoint) here
            generator.BindLabel(elseEntryPoint);

            // Generate the label for the (pending) goto instruction
            var elseExitPoint = generator.ProgramBuilder.NewLabel();
            @goto.SetDestination(elseExitPoint);
            
            var right = nullc.Right.Exec(generator);
            generator.Emmit(new StoreInstruction(retval, right));

            // Finally, backpatch the goto to jump from the then's body to avoid
            // fall through the else's body
            generator.BindLabel(elseExitPoint);

            return retval;
        }
    }
}
