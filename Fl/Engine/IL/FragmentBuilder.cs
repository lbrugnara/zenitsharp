// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.VM;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.IL
{
    public class FragmentBuilder
    {
        public string Name { get; }
        public FragmentType Type { get; }
        public List<Instruction> _instructions;

        public FragmentBuilder(string name, FragmentType type)
        {
            this.Name = name;
            this.Type = type;
            _instructions = new List<Instruction>();
        }

        public void AddInstruction(Instruction i)
        {
            _instructions.Add(i);
        }

        public Fragment Build() => new Fragment(Name, Type, _instructions);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Type == FragmentType.Global)
                sb.AppendLine($"{Name}:");
            else
                sb.AppendLine($"{Type.ToString().ToLower()} {Name}:");

            for (int i = 0; i < _instructions.Count; i++)
            {
                var instruction = _instructions[i];
                if (instruction.Label != null)
                    sb.AppendLine($"{instruction.Label}");
                sb.AppendLine($"{i.ToString().PadLeft(6, ' ')}: {instruction.ToString()}");
            }
            return sb.ToString();
        }
    }
}
