// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.IL.Instructions
{
    public class UnaryInstruction : Instruction
    {
        public Operand Left { get; }

        public UnaryInstruction(OpCode opcode, SymbolOperand tempName, Operand left)
            : base(opcode, tempName)
        {
            this.Left = left;
        }

        public override string ToString()
        {
            return $"{this.DestSymbol} = {this.OpCode.InstructionName()} {this.Left}";
        }
    }
}
