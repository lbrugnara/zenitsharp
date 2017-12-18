// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
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
