// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class PostDecInstruction : AssignInstruction
    {
        public PostDecInstruction(SymbolOperand tempName)
            : base(OpCode.PostDec, tempName)
        {
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.Destination}";
        }
    }
}
