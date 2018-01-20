using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.IL.Instructions.Exceptions
{
    public class InvalidInstructionException : Exception
    {
        public InvalidInstructionException(string message)
            : base(message)
        {
        }

        public InvalidInstructionException(OpCode opcode)
            : base($"Invalid usage of opcode {opcode.InstructionName()}")
        {
        }
    }
}
