// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Fl.Engine.IL
{
    public class Fragment
    {
        public string Name { get; }
        public FragmentType Type { get; }
        public List<Instruction> _instructions;

        public Fragment(string name, FragmentType type)
        {
            this.Name = name;
            this.Type = type;
            _instructions = new List<Instruction>();
        }

        public ReadOnlyCollection<Instruction> Instructions => new ReadOnlyCollection<Instruction>(_instructions);

        public int NextAddress => _instructions.Count;

        public void AddInstruction(Instruction i)
        {
            _instructions.Add(i);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Type == FragmentType.Global)
                sb.AppendLine($"{Name}:");
            else
                sb.AppendLine($"{Type.ToString().ToLower()} {Name}:");

            var instructions = Instructions;
            for (int i = 0; i < instructions.Count; i++)
            {
                var instruction = instructions[i];
                sb.AppendLine($"{i.ToString().PadLeft(6, ' ')}: {instruction.ToString()}");
            }
            return sb.ToString();
        }
    }
}
