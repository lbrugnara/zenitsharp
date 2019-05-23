// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Zenit.Semantics.Symbols.Containers
{
    public class Loop : Block, ILoop
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
