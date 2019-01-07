// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols
{
    public class BlockSymbolContainer : SymbolContainer
    {
        public BlockSymbolContainer(string name)
            : base(name)
        {
        }

        public BlockSymbolContainer(string name, SymbolContainer parent)
            : base(name, parent)
        {
        }

        public override string ToString()
        {
            return $"block-{this.Name}";
        }
    }
}
