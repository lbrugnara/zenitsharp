﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Zenit.IL.VM
{
    public class Fragment
    {
        public string Name { get; }
        public FragmentType Type { get; }
        public ReadOnlyCollection<Instruction> Instructions { get; }
        public Dictionary<string, int> Labels { get; }

        public Fragment(string name, FragmentType type, List<Instruction> instructions)
        {
            this.Name = name;
            this.Type = type;
            this.Instructions = new ReadOnlyCollection<Instruction>(instructions);
            this.Labels = new Dictionary<string, int>();

            for (int i=0; i < instructions.Count; i++)
            {
                if (instructions[i].Label == null)
                    continue;
                this.Labels[instructions[i].Label.Name] = i;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Type == FragmentType.Global)
                sb.AppendLine($"{Name}:");
            else
                sb.AppendLine($"{Type.ToString().ToLower()} {Name}:");

            for (int i = 0; i < Instructions.Count; i++)
            {
                var instruction = Instructions[i];
                if (instruction.Label != null)
                    sb.AppendLine($"{instruction.Label}");
                sb.AppendLine($"{i.ToString().PadLeft(6, ' ')}: {instruction.ToString()}");
            }
            return sb.ToString();
        }
    }
}
