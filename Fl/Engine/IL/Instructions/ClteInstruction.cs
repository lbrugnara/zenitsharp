// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class ClteInstruction : Instruction
    {
        public Operand Left { get; }
        public Operand Right { get; }

        public ClteInstruction(SymbolOperand tempName, Operand left, Operand right)
            : base(OpCode.Clte, tempName)
        {
            this.Left = left;
            this.Right = right;
        }

        public override string ToString()
        {
            return $"{this.DestSymbol} = {this.OpCode.InstructionName()} {this.Left} {this.Right}";
        }
    }
}
