// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.IL.Instructions
{
    public class ConstInstruction : AssignInstruction
    {
        public OperandType TypeResolver { get; }
        public Operand Value { get; }

        public ConstInstruction(SymbolOperand name, OperandType typeres, Operand value)
            : base(OpCode.Const, name)
        {
            this.TypeResolver = typeres;
            this.Value = value;
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.Destination} = {this.Value}";
        }
    }
}
