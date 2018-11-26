// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Types
{
    public class Function : Complex
    {
        /// <summary>
        /// Function's parameters types
        /// </summary>
        public List<Object> Parameters { get; private set; }

        /// <summary>
        /// Function's return type
        /// </summary>
        public Object Return { get; private set; }

        public Function()
            : base("func")
        {
            this.Parameters = new List<Object>();
        }

        public Function(params Object[] parametersTypes)
            : base("func")
        {
            this.Parameters = parametersTypes?.ToList() ?? new List<Object>();
        }

        /// <summary>
        /// Define a new parameter type
        /// </summary>
        /// <param name="type"></param>
        public void DefineParameterType(Object type) => this.Parameters.Add(type);

        /// <summary>
        /// Set the return type
        /// </summary>
        /// <param name="ret"></param>
        public void SetReturnType(Object ret) => this.Return = ret;

        public bool IsCircularReference => this.HasCircularReferenceWith(this.Return);

        protected bool HasCircularReferenceWith(Object type)
        {
            if (type is Function functype)
            {
                if (this.GetHashCode() == functype.GetHashCode())
                    return true;
                return this.HasCircularReferenceWith(functype.Return);
            }

            if (type is Tuple tuptype)
                return tuptype.Types.Any(t => this.HasCircularReferenceWith(t));

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj) || !(obj is Function func))
                return false;

            if (!this.Parameters.SequenceEqual(func.Parameters))
                return false;

            if (this.IsCircularReference && func.IsCircularReference && this.GetHashCode() == func.GetHashCode())
                return true;

            return this.Return.GetHashCode() == func.GetHashCode() || this.Return == func.Return;
        }

        public static bool operator ==(Function type1, Object type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Function type1, Object type2)
        {
            return !(type1 == type2);
        }

        public override string ToSafeString(List<(Object type, string safestr)> safeTypes)
        {
            var parameters = this.Parameters
                            .Select(s => s?.ToString() ?? "?")
                            .ToList();

            var str = base.ToString() + "(" + string.Join(", ", parameters) + $"):";

            str += " ";
            if (safeTypes.Any(st => st.type == this.Return))
            {
                str += safeTypes.First(st => st.type == this.Return).safestr;
            }
            else if (this.Return is Complex stype)
            {
                safeTypes.Add((this, "...<cyclic func ref>"));
                str += stype.ToSafeString(safeTypes);
            }
            else
            {
                str += this.Return?.ToString() ?? "void";
            }

            return str;
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
