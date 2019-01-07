// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols
{
    public class LoopSymbolContainer : SymbolContainer
    {
        public LoopSymbolContainer(string name, SymbolContainer parent)
            : base(name, parent)
        {
        }

        public override string ToString()
        {
            return $"loop-{this.Name}";
        }
    }
}
