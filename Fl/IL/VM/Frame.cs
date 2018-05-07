// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.IL.VM
{
    public class Frame
    {
        public Stack<FlObject> Parameters { get; }
        public InstructionPointer InstrPointer { get; }

        public Frame(string fragment)
        {
            this.Parameters = new Stack<FlObject>();
            this.InstrPointer = new InstructionPointer(fragment);
        }

        public Frame(string fragment, Stack<FlObject> parameters)
        {
            this.Parameters = parameters;
            this.InstrPointer = new InstructionPointer(fragment);
        }
    }
}
