// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Types
{
    public class Class : Struct
    {
        public Dictionary<string, ClassProperty> Properties { get; }
        public Dictionary<string, ClassMethod> Methods { get; }

        public Class()
            : base("class")
        {
            this.Properties = new Dictionary<string, ClassProperty>();
            this.Methods = new Dictionary<string, ClassMethod>();
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

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj) || !(obj is Class classObj))
                return false;

            return this.Properties.SequenceEqual(classObj.Properties)
                    && this.Methods.SequenceEqual(classObj.Methods);
        }

        public override bool IsAssignableFrom(Type type)
        {
            return this.Equals(type);
        }
    }
}
