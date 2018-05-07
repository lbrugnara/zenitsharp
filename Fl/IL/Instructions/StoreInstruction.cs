// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;

namespace Fl.IL.Instructions
{
    public class StoreInstruction : AssignInstruction
    {
        public Operand Value { get; }

        public StoreInstruction(SymbolOperand toName, Operand value)
            : base(OpCode.Store, toName)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.Destination} = {this.Value}";
        }
    }
}
