// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;

namespace Zenit.IL.Instructions
{
    public class CallInstruction : Instruction
    {
        public Operand Func { get; }
        public int NumberOfParams { get; }

        public CallInstruction(Operand fn, int nparams)
            : base (OpCode.Call)
        {
            this.Func = fn;
            this.NumberOfParams = nparams;
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.Func} {this.NumberOfParams}";
        }
    }
}
