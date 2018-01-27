﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class LocalInstruction : AssignInstruction
    {
        public LocalInstruction(SymbolOperand local)
            : base(OpCode.Local, local)
        {
        }

        public override string ToString()
        {
            return $"local {this.Destination}";
        }
    }
}
