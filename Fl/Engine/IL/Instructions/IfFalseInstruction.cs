// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL.Instructions
{
    public class IfFalseInstruction : Instruction
    {
        public Operand Condition { get; }
        public Label Goto { get; }

        public IfFalseInstruction(Operand condition, Label @goto)
            : base(OpCode.IfFalse)
        {
            this.Condition = condition;
            this.Goto = @goto;
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {Condition} goto {Goto}";
        }
    }
}
