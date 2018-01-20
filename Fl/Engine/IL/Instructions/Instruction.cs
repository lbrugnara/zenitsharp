// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;

namespace Fl.Engine.IL.Instructions
{
    public abstract class Instruction
    {
        public SymbolOperand DestSymbol { get; }
        public OpCode OpCode { get; }

        public Instruction(OpCode opCode, SymbolOperand destName = null)
        {
            this.OpCode = opCode;
            this.DestSymbol = destName;
        }
    }
}
