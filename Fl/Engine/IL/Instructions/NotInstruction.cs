// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class NotInstruction : AssignInstruction
    {
        public Operand Left { get; }

        public NotInstruction(SymbolOperand tempName, Operand left)
            : base(OpCode.Not, tempName)
        {
            this.Left = left;
        }

        public override string ToString()
        {
            return $"{this.Destination} = {this.OpCode.InstructionName()} {this.Left}";
        }
    }
}
