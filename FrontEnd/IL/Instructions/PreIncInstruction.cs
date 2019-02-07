// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;

namespace Zenit.IL.Instructions
{
    public class PreIncInstruction : AssignInstruction
    {
        public Operand Left { get; }

        public PreIncInstruction(SymbolOperand tempName, SymbolOperand left)
            : base(OpCode.PreInc, tempName)
        {
            this.Left = left;
        }

        public override string ToString()
        {
            return $"{this.Destination} = {this.OpCode.InstructionName()} {this.Left}";
        }
    }
}
