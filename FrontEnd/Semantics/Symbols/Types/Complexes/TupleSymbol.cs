﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Zenit.Semantics.Symbols
{
    public class TupleSymbol : ComplexSymbol
    {
        public List<ITypeSymbol> Types { get; }

        public TupleSymbol(IContainer parent, List<ITypeSymbol> types)
            : base("tuple", BuiltinType.Tuple, parent)
        {
            this.Types = types ?? new List<ITypeSymbol>();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && this.Types.SequenceEqual((obj as TupleSymbol).Types);
        }

        public int Count => this.Types.Count;

        public static bool operator ==(TupleSymbol type1, TupleSymbol type2)
        {
            if (type1 is null)
                return type2 is null;

            return type1.Equals(type2);
        }

        public static bool operator !=(TupleSymbol type1, TupleSymbol type2)
        {
            return !(type1 == type2);
        }

        public override string ToValueString()
        {
            return this.ToSafeString((this, "self"));
        }

        public override string ToSafeString(params (ITypeSymbol type, string safestr)[] safeTypes)
        {
            var types = this.Types.Select(t =>
            {
                if (safeTypes.Any(st => st.type == t))
                    return safeTypes.First(st => st.type == t).safestr;

                if (t is TupleSymbol ttype)
                    return ttype.ToSafeString(safeTypes);
                else if (t is FunctionSymbol ftype)
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