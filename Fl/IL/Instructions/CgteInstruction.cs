// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;

namespace Fl.IL.Instructions
{
    public class CgteInstruction : AssignInstruction
    {
        public Operand Left { get; }
        public Operand Right { get; }

        public CgteInstruction(SymbolOperand tempName, Operand left, Operand right)
            : base(OpCode.Cgte, tempName)
        {
            this.Left = left;
            this.Right = right;
        }

        public override string ToString()
        {
            return $"{this.Destination} = {this.OpCode.InstructionName()} {this.Left} {this.Right}";
        }
    }
}
