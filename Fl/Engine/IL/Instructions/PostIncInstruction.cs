// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class PostIncInstruction : Instruction
    {
        public PostIncInstruction(SymbolOperand tempName)
            : base(OpCode.PostInc, tempName)
        {
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.DestSymbol}";
        }
    }
}
