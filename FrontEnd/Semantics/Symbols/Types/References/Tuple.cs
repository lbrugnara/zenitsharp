// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Types;
using System.Collections.Generic;
using System.Linq;
using System;
using Zenit.Semantics.Symbols.Variables;

namespace Zenit.Semantics.Symbols.Types.References
{
    public class Tuple : Reference
    {
        public Tuple(string name, IContainer parent, List<ISymbol> elements = null)
            : base(name, BuiltinType.Tuple, parent)
        {
            if (elements != null)
            {
                for (int i = 0; i < elements.Count; i++)
                    this.Insert(new Variable($"${i}", elements[i].GetTypeSymbol(), Access.Public, Storage.Immutable, this));
            }
        }

        public int Count => this.Elements.Count;

        public List<IVariable> Elements => this.GetAllOfType<IVariable>();

        public override bool Equals(object obj)
        {
            var tuple2 = obj as Tuple;

            if (tuple2 == null || !base.Equals(obj))
                return false;

            if (this.Count != tuple2.Count)
                return false;

            return this.Elements.SequenceEqual(tuple2.Elements);
        }

        public static bool operator ==(Tuple type1, Tuple type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(Tuple type1, Tuple type2)
        {
            return !(type1 == type2);
        }

        public override string ToValueString()
        {
            return this.ToSafeString((this, "self"));
        }

        public override string ToSafeString(params (IType type, string safestr)[] safeTypes)
        {
            var types = this.Elements.Select(s =>
            {
                var t = s.GetTypeSymbol();

                if (safeTypes.Any(st => st.type == t))
                    return safeTypes.First(st => st.type == t).safestr;

                if (t is Tuple ttype)
                    return ttype.ToSafeString(safeTypes);
                else if (t is Function ftype)
                    return ftype.ToSafeString(safeTypes);

                return t.ToValueString() ?? "?";
            }).ToList();

            return $"({string.Join(", ", types)})";
        }

        public override string ToString()
        {
            return this.ToSafeString((this, "self"));
        }
    }
}
