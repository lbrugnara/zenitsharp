// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;

namespace Fl.IL.Instructions
{
    public class NegInstruction : AssignInstruction
    {
        public Operand Left { get; }

        public NegInstruction(SymbolOperand tempName, Operand left)
            : base(OpCode.Neg, tempName)
        {
            this.Left = left;
        }

        public override string ToString()
        {
            return $"{this.Destination} = {this.OpCode.InstructionName()} {this.Left}";
        }
    }
}
