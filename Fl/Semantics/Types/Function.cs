// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Types
{
    public class Function : Struct
    {
        /// <summary>
        /// Function's parameters types
        /// </summary>
        public List<Type> Parameters { get; private set; }

        /// <summary>
        /// Function's return type
        /// </summary>
        public Type Return { get; private set; }

        public Function()
            : base("func")
        {
            this.Parameters = new List<Type>();
        }

        public Function(Type returnType, params Type[] parametersTypes)
            : base("func")
        {
            this.Parameters = parametersTypes?.ToList() ?? new List<Type>();
            this.Return = returnType ?? throw new System.ArgumentNullException(nameof(returnType), "Return type cannot be null");
        }

        /// <summary>
        /// Define a new parameter type
        /// </summary>
        /// <param name="type"></param>
        public void DefineParameterType(Type type) => this.Parameters.Add(type);

        /// <summary>
        /// Set the return type
        /// </summary>
        /// <param name="ret"></param>
        public void SetReturnType(Type ret) => this.Return = ret;

        public bool IsCircularReference
        {
            get
            {
                if (this.Return is Function && this.GetHashCode() == this.Return?.GetHashCode())
                    return true;

                if (this.Return is Tuple tuple && tuple.Types.Any(t => t is Function f && this.GetHashCode() == f.GetHashCode()))
                    return true;

                return false;
            }
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

        public static bool operator ==(Function type1, Type type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Function type1, Type type2)
        {
            return !(type1 == type2);
        }

        public override string ToString()
        {
            var parameters = this.Parameters
                            .Select(s => s?.ToString() ?? "?")
                            .ToList();

            var str = base.ToString() + "(" + string.Join(", ", parameters) + $"):";

            if (this.IsCircularReference)
            {
                if (this.Return is Tuple tuple)
                {
                    var tupleTypes = new List<string>();
                    tuple.Types.ForEach(t =>
                    {
                        if (t == this)
                            tupleTypes.Add(str + "<circular reference>");
                        else
                            tupleTypes.Add(t.ToString());
                    });

                    str += $" tuple({string.Join(", ", tupleTypes)})";
                }
                else
                {
                    str += "<circular reference>";
                }
            }
            else
            {
                str += " " + this.Return?.ToString() ?? "?";
            }

            return str;
        }

        public override bool IsAssignableFrom(Type type)
        {
            return this.Equals(type);
        }
    }
}
