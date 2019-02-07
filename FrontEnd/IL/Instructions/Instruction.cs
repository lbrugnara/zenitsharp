// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Zenit.IL.Instructions
{
    public abstract class Instruction
    {
        public OpCode OpCode { get; }
        public Label Label { get; set; }

        public Instruction(OpCode opCode)
        {
            this.OpCode = opCode;
        }
    }
}
