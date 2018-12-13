// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Semantics.Types
{
    public abstract class Primitive : Object
    {
        public Primitive(string name)
            : base(name)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Primitive && (obj as Primitive).Name == this.Name;
        }
    }
}
