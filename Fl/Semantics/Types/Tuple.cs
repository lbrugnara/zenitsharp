// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Types
{
    public class Tuple : Complex
    {
        public List<Object> Types { get; set; }

        private Tuple()
            : base("tuple")
        {
            this.Types = new List<Object>();
        }

        public Tuple(params Object[] types)
            : base("tuple")
        {
            this.Types = types?.ToList() ?? new List<Object>();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && this.Types.SequenceEqual((obj as Tuple).Types);
        }

        public int Count => this.Types.Count;

        public static bool operator ==(Tuple type1, Object type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Tuple type1, Object type2)
        {
            return !(type1 == type2);
        }

        public override string ToSafeString(List<(Object type, string safestr)> safeTypes)
        {
            var types = this.Types.Select(t =>
            {
                if (safeTypes.Any(st => st.type == t))
                    return safeTypes.First(st => st.type == t).safestr;

                if (t is Complex stype)
                    return stype.ToSafeString(safeTypes);

                return t.ToString() ?? "?";
            });

            return base.ToString() + "(" + string.Join(", ", types) + ")";
        }

        public override string ToString()
        {
            return this.ToSafeString(new List<(Object type, string safestr)>());
        }

        public override bool IsAssignableFrom(Object type)
        {
            return this.Equals(type);
        }
    }
}
