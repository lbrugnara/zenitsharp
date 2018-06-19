// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;

namespace Fl.Symbols.Types
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

        /// <summary>
        /// Define a new parameter type
        /// </summary>
        /// <param name="type"></param>
        public void DefineParameterType(Type type) => this.Parameters.Add(type);

        /// <summary>
        /// Set the return type
        /// </summary>
        /// <param name="ret"></param>
        public void SetReturnType(Type ret) =>  this.Return = ret;

        public override bool Equals(object obj)
        {
            return base.Equals(obj) 
                && this.Return == (obj as Function).Return
                && this.Parameters.SequenceEqual((obj as Function).Parameters);
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
            return base.ToString() + "(" + string.Join(", ", parameters) + $"): {(this.Return?.ToString() ?? "?")}";
        }

        public override bool IsAssignableFrom(Type type)
        {
            return this.Equals(type);
        }
    }
}
