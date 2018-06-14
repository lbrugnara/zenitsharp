// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

namespace Fl.Symbols.Types
{
    public abstract class SType : Symbol
    {
        private SType()
            : base("type", null)
        {
            this.Name = "type";
        }

        public SType(string name)
            : base (name, null)
        {
            this.Name = name;
        }

        public override SType Type { get => this; set => base.Type = this; }

        public override bool Equals(object obj)
        {
            return this.Name == (obj as SType)?.Name;
        }

        public static bool operator ==(SType type1, SType type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(SType type1, SType type2)
        {
            return !(type1 == type2);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public virtual bool IsAssignableFrom(SType type)
        {
            return this == type;
        }
    }
}
