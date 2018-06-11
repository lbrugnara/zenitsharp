// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Linq;

namespace Fl.Lang.Types
{
    public class Func : ComplexType
    {
        public Type[] Parameters { get; private set; }
        public Type Return { get; set; }

        public Func(Type[] parameters, Type returns)
            : base("func")
        {
            this.Parameters = parameters ?? new Type[] { };
            this.Return = returns;
        }

        public Func()
            : base ("func")
        {
            this.Parameters = new Type[] { };
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) 
                && this.Return == (obj as Func).Return
                && this.Parameters.SequenceEqual((obj as Func).Parameters);
        }

        public static bool operator ==(Func type1, Type type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Func type1, Type type2)
        {
            return !(type1 == type2);
        }

        public override string ToString()
        {
            return base.ToString() + "(" + string.Join<Type>(", ", this.Parameters) + $"): {this.Return}";
        }

        public override bool IsAssignableFrom(Type type)
        {
            return this.Equals(type);
        }
    }
}
