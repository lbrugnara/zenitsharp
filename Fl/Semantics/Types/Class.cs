// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols;

namespace Fl.Semantics.Types
{
    public class Class : Struct
    {
        public Class()
            : base("class")
        {
        }

        public static bool operator ==(Class type1, Type type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Class type1, Type type2)
        {
            return !(type1 == type2);
        }

        public override bool IsAssignableFrom(Type type)
        {
            return this.Equals(type);
        }
    }
}
