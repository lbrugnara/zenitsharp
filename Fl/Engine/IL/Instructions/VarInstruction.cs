// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.IL.Instructions
{
    public class VarInstruction : Instruction
    {        
        public string Type { get; }
        public Operand Value { get; }

        public VarInstruction(SymbolOperand name, string type, Operand value)
            : base(OpCode.Var, name)
        {
            this.Type = type;
            this.Value = value;
        }

        public override string ToString()
        {
            if (this.Value != null)
                return $"{this.OpCode.InstructionName()} {this.DestSymbol} {this.Value}";
            return $"{this.OpCode.InstructionName()} {this.DestSymbol} {FlNull.Value}";
        }
    }
}
