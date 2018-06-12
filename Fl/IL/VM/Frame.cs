// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.IL.VM
{
    public class Frame
    {
        public Stack<object> Parameters { get; }
        public InstructionPointer InstrPointer { get; }

        public Frame(string fragment)
        {
            this.Parameters = new Stack<object>();
            this.InstrPointer = new InstructionPointer(fragment);
        }

        public Frame(string fragment, Stack<object> parameters)
        {
            this.Parameters = parameters;
            this.InstrPointer = new InstructionPointer(fragment);
        }
    }
}
