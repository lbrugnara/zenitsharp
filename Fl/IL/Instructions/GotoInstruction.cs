﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.IL.Instructions
{
    public class GotoInstruction : Instruction
    {
        public Label Goto { get; private set; }

        public GotoInstruction(Label @goto)
            : base(OpCode.Goto)
        {
            this.Goto = @goto;
        }

        public void SetDestination(Label @goto)
        {
            this.Goto = @goto;
        }

        public override string ToString()
        {
            return $"{this.OpCode.InstructionName()} {Goto}";
        }
    }
}