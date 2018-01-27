﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class MultInstruction : AssignInstruction
    {
        public Operand Left { get; }
        public Operand Right { get; }

        public MultInstruction(SymbolOperand tempName, Operand left, Operand right)
            : base(OpCode.Mult, tempName)
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
