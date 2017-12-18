// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Engine.IL.Instructions.Operands
{
    public class SymbolOperand : Operand
    {
        public string Name { get; }
        public SymbolOperand Member { get; private set; }

        public SymbolOperand(string name, string type = null)
            : base (type)
        {
            this.Name = name;
        }

        public void AddMember(SymbolOperand member)
        {
            if (this.Member == null)
                this.Member = member;
            else
                this.Member.AddMember(member);
        }

        public override string ToString()
        {
            string s = this.Name;
            if (this.Member != null)
                s += $".{this.Member}";
            return s;
        }
    }
}
