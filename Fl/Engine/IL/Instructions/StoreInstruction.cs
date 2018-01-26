// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class StoreInstruction : Instruction
    {
        public Operand Value { get; }

        public StoreInstruction(SymbolOperand toName, Operand value)
            : base(OpCode.Store, toName)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.DestSymbol} = {this.Value}";
        }
    }
}
