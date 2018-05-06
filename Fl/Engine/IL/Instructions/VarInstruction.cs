// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.IL.Instructions
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
            return $"{this.OpCode.InstructionName()} {this.Destination} = {FlNull.Value}";
        }
    }
}
