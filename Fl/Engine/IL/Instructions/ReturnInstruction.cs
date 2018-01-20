// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Engine.IL.Instructions
{
    public class ReturnInstruction : Instruction
    {
        public ReturnInstruction()
            : base(OpCode.Return)
        {
        }

        public override string ToString()
        {
            return this.OpCode.InstructionName();
        }
    }
}
