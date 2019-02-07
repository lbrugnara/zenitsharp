// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;
using Zenit.Engine.Symbols.Types;

namespace Zenit.IL.Instructions
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
