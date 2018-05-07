// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;

namespace Fl.IL.Instructions
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
