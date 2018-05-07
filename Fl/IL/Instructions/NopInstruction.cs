// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.IL.Instructions
{
    public class NopInstruction : Instruction
    {
        public NopInstruction() 
            : base(OpCode.Nop)
        {
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()}";
        }
    }
}
