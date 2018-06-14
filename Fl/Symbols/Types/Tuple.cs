// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;

namespace Fl.Symbols.Types
{
    public class Tuple : Struct
    {
        public List<SType> Types { get; set; }

        private Tuple()
            : base("tuple")
        {
            this.Types = new List<SType>();
        }

        public Tuple(params SType[] types)
            : base("tuple")
        {
            this.Types = types?.ToList() ?? new List<SType>();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && this.Types.SequenceEqual((obj as Tuple).Types);
        }

        public int Count => this.Types.Count;

        public static bool operator ==(Tuple type1, SType type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Tuple type1, SType type2)
        {
            return !(type1 == type2);
        }

        public override string ToString()
        {
            return base.ToString() + "(" + string.Join(", ", this.Types) + ")";
        }

        public override bool IsAssignableFrom(SType type)
        {
            return this.Equals(type);
        }
    }
}
