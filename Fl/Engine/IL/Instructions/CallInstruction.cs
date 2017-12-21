// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class CallInstruction : Instruction
    {
        public Operand Func { get; }
        public int NumberOfParams { get; }

        public CallInstruction(SymbolOperand destination, Operand fn, int nparams)
            : base (OpCode.Call, destination)
        {
            this.Func = fn;
            this.NumberOfParams = nparams;
        }

        public override string ToString()
        {
            return $"{this.DestSymbol} = {this.OpCode.InstructionName()} {this.Func} {this.NumberOfParams}";
        }
    }
}
