// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.IL.VM
{
    public class InstructionPointer
    {
        public int IP { get; set; }
        public string FragmentName { get; set; }

        public InstructionPointer(string fragmentName)
        {
            this.FragmentName = fragmentName;
            this.IP = 0;
        }
    }
}
