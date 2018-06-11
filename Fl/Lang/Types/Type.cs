// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Lang.Types
{
    public abstract class PrimitiveType : Type
    {
        public PrimitiveType(string name)
            : base(name)
        {
        }
    }

    public abstract class ComplexType : Type
    {
        public ComplexType(string name)
            : base(name)
        {
        }
    }

    public abstract class Type
    {
        private string Name { get; }

        private Type()
        {
            this.Name = "type";
        }

        public Type(string name)
        {
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            return this.Name == (obj as Type)?.Name;
        }

        public static bool operator ==(Type type1, Type type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Type type1, Type type2)
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

        public virtual bool IsAssignableFrom(Type type)
        {
            return this == type;
        }
    }
}
