// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols.Containers
{
    public class Loop : Block
    {
        public Loop(string name, IContainer parent)
            : base(name, parent)
        {
        }

        public override string ToString()
        {
            return $"loop-{this.Name}";
        }
    }
}
