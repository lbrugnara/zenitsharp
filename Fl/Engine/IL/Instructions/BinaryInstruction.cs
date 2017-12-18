// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.IL.Instructions
{
    public class BinaryInstruction : Instruction
    {
        public Operand Left { get; }
        public Operand Right { get; }

        public BinaryInstruction(OpCode opcode, SymbolOperand tempName, Operand left, Operand right)
            : base(opcode, tempName)
        {
            this.Left = left;
            this.Right = right;
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {this.DestSymbol} {this.Left} {this.Right}";
        }
    }
}
