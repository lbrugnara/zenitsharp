// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class ConstInstruction : Instruction
    {
        public string Type { get; }
        public Operand Value { get; }

        public ConstInstruction(SymbolOperand name, string type, Operand value)
            : base(OpCode.Const, name)
        {
            this.Type = type;
            this.Value = value;
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.DestSymbol} {this.Value}";
        }
    }
}
