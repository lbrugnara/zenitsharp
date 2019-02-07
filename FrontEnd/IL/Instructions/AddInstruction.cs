// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;

namespace Zenit.IL.Instructions
{
    public class AddInstruction : AssignInstruction
    {
        public Operand Left { get; }
        public Operand Right { get; }

        public AddInstruction(SymbolOperand tempName, Operand left, Operand right)
            : base(OpCode.Add, tempName)
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
