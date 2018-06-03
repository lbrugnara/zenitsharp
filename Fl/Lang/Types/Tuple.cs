﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Lang.Types
{
    public class Tuple : Type
    {
        public static Tuple Instance { get; } = new Tuple();

        private List<Type> Types;

        private Tuple()
            : base("tuple")
        {
            this.Types = new List<Type>();
        }

        public Tuple(params Type[] types)
            : base("tuple")
        {
            this.Types = types?.ToList() ?? new List<Type>();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && this.Types.SequenceEqual((obj as Tuple).Types);
        }

        public static bool operator ==(Tuple type1, Type type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Tuple type1, Type type2)
        {
            return !(type1 == type2);
        }

        public override string ToString()
        {
            return base.ToString() + "(" + string.Join(", ", this.Types) + ")";
        }

        public override bool IsAssignableFrom(Type type)
        {
            return this.Equals(type);
        }
    }
}