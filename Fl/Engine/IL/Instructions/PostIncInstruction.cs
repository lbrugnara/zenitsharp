// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class PostIncInstruction : AssignInstruction
    {
        public Operand Left { get; }

        public PostIncInstruction(SymbolOperand tempName, SymbolOperand left)
            : base(OpCode.PostInc, tempName)
        {
            this.Left = left;
        }

        public override string ToString()
        {
            return $"{this.Destination} = {this.OpCode.InstructionName()} {this.Left}";
        }
    }
}
