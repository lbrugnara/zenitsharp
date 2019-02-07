// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;

namespace Zenit.IL.Instructions
{
    public class ParamInstruction : Instruction
    {
        public Operand Parameter { get; }

        public ParamInstruction(Operand param)
            : base (OpCode.Param)
        {
            this.Parameter = param;
        }

        public override string ToString()
        {
            return $"param {this.Parameter}";
        }
    }
}
