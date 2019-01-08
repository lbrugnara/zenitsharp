// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols
{
    public class Loop : Block
    {
        public Loop(string name, ISymbolContainer parent)
            : base(name, parent)
        {
        }

        public override string ToString()
        {
            return $"loop-{this.Name}";
        }
    }
}
