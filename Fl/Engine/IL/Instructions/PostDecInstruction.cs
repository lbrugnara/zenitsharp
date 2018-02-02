// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class PostDecInstruction : AssignInstruction
    {
        public Operand Left { get; }

        public PostDecInstruction(SymbolOperand tempName, SymbolOperand left)
            : base(OpCode.PostDec, tempName)
        {
            this.Left = left;
        }

        public override string ToString()
        {
            return $"{this.Destination} = {this.OpCode.InstructionName()} {this.Left}";
        }
    }
}
