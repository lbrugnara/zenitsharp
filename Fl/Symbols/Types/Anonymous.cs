﻿namespace Fl.Symbols.Types
{
    public class Anonymous : Primitive
    {
        private string name;
        
        public Anonymous(string name)
            : base("'")
        {
            this.name = name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && this.name == (obj as Anonymous).name;
        }

        public static bool operator ==(Anonymous type1, SType type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Anonymous type1, SType type2)
        {
            return !(type1 == type2);
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this.name}";
        }

        public override bool IsAssignableFrom(SType type)
        {
            return this.Equals(type);
        }
    }
}