// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Engine.IL
{
    public class Label
    {
        public int Address { get; set; }

        public override string ToString()
        {
            return Address == -1 ? "<unknown>" : Address.ToString();
        }
    }
}
