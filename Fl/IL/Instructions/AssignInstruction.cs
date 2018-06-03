﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;

namespace Fl.IL.Instructions
{
    public abstract class AssignInstruction : Instruction
    {
        public SymbolOperand Destination { get; }

        public AssignInstruction(OpCode opcode, SymbolOperand destName)
            : base (opcode)
        {
            this.Destination = destName;
        }
    }
}