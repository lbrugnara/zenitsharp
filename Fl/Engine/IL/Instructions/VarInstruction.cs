// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.IL.Instructions
{
    public class VarInstruction : Instruction
    {        
        public TypeResolver TypeResolver { get; }
        public Operand Value { get; }

        public VarInstruction(SymbolOperand name, TypeResolver type, Operand value = null)
            : base(OpCode.Var, name)
        {
            this.TypeResolver = type;
            this.Value = value;
        }

        public override string ToString()
        {
            if (this.Value != null)
                return $"{this.OpCode.InstructionName()} {this.DestSymbol} = {this.Value}";
            return $"{this.OpCode.InstructionName()} {this.DestSymbol} = {FlNull.Value}";
        }
    }
}
