﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;

namespace Zenit.IL.Instructions
{
    public class SubInstruction : AssignInstruction
    {
        public Operand Left { get; }
        public Operand Right { get; }

        public SubInstruction(SymbolOperand tempName, Operand left, Operand right)
            : base(OpCode.Sub, tempName)
        {
            this.Left = left;
            this.Right = right;
        }

        public override string ToString()
        {
            return $"{this.Destination} = {this.OpCode.InstructionName()} {this.Left} {this.Right}";
        }
    }
}
