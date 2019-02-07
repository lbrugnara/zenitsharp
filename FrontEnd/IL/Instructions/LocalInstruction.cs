// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;

namespace Zenit.IL.Instructions
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
