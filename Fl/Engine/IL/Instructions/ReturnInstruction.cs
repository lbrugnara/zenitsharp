// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class ReturnInstruction : AssignInstruction
    {
        public ReturnInstruction(SymbolOperand destination)
            : base(OpCode.Return, destination)
        {
        }

        public override string ToString()
        {
            if (Destination == null)
                return this.OpCode.InstructionName();
            return $"{this.OpCode.InstructionName()} {Destination}";
        }
    }
}
