// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class PreIncInstruction : Instruction
    {
        public PreIncInstruction(SymbolOperand tempName)
            : base(OpCode.PreInc, tempName)
        {
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.DestSymbol}";
        }
    }
}
