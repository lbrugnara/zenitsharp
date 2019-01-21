// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file



namespace Fl.Semantics.Symbols.Containers
{
    public class Block : Container
    {
        public Block(string name)
            : base(name, null)
        {
        }

        public Block(string name, IContainer parent)
            : base(name, parent)
        {
        }

        public override string ToString()
        {
            return $"container-{this.Name}";
        }
    }
}
