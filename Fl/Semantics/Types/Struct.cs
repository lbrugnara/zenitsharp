// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using System.Collections.Generic;

namespace Fl.Semantics.Types
{
    public abstract class Struct
    {
        public string Name { get; private set; }

        public Dictionary<string, Struct> Properties { get; }
        public Dictionary<string, Function> Methods { get; }
        private Struct()
            : this ("struct")
        {
        }

        public Struct(string name)
        {
            this.Name = name;
            this.Properties = new Dictionary<string, Struct>();
            this.Methods = new Dictionary<string, Function>();
        }

        public override bool Equals(object obj)
        {
            return this.Name == (obj as Struct)?.Name;
        }

        public static bool operator ==(Struct type1, Struct type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Struct type1, Struct type2)
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

        public virtual bool IsAssignableFrom(Struct type)
        {
            return this == type;
        }
    }
}
