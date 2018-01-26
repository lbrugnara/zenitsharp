// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class PreDecInstruction : Instruction
    {
        public PreDecInstruction(SymbolOperand tempName)
            : base(OpCode.PreDec, tempName)
        {
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.DestSymbol}";
        }
    }
}
