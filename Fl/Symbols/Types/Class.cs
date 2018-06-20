// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using System.Linq;

namespace Fl.Symbols.Types
{
    public class Class : Struct
    {
        public Scope Methods { get; set; }
        public Scope Properties { get; set; }

        public Class()
            : base("class")
        {
            this.Methods = new Scope(ScopeType.Common, "@methods");
            this.Properties = new Scope(ScopeType.Common, "@properties");
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) 
                && this.Methods.Equals((obj as Class).Methods)
                && this.Properties.Equals((obj as Class).Properties);
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
