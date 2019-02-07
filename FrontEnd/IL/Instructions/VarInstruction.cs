// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;
using Zenit.Engine.Symbols.Types;

namespace Zenit.IL.Instructions
{
    public class VarInstruction : AssignInstruction
    {        
        public OperandType Type { get; }
        public Operand Value { get; }

        public VarInstruction(SymbolOperand name, OperandType type, Operand value = null)
            : base(OpCode.Var, name)
        {
            this.Type = type;
            this.Value = value;
        }

        public override string ToString()
        {
            if (this.Value != null)
                return $"{this.OpCode.InstructionName()} {this.Destination} = {this.Value}";
            return $"{this.OpCode.InstructionName()} {this.Destination} = null";
        }
    }
}
