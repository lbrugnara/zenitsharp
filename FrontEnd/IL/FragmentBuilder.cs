// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions;
using Zenit.IL.VM;
using System.Collections.Generic;
using System.Text;

namespace Zenit.IL
{
    public class FragmentBuilder
    {
        public string Name { get; }
        public FragmentType Type { get; }
        public List<Instruction> Instructions { get; }

        public FragmentBuilder(string name, FragmentType type)
        {
            this.Name = name;
            this.Type = type;
            this.Instructions = new List<Instruction>();
        }

        public void AddInstruction(Instruction i)
        {
            this.Instructions.Add(i);
        }

        public Fragment Build() => new Fragment(this.Name, this.Type, this.Instructions);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (this.Type == FragmentType.Global)
                sb.AppendLine($"{this.Name}:");
            else
                sb.AppendLine($"{this.Type.ToString().ToLower()} {this.Name}:");

            for (int i = 0; i < this.Instructions.Count; i++)
            {
                var instruction = this.Instructions[i];
                if (instruction.Label != null)
                    sb.AppendLine($"{instruction.Label}");
                sb.AppendLine($"{i.ToString().PadLeft(6, ' ')}: {instruction.ToString()}");
            }
            return sb.ToString();
        }
    }
}
