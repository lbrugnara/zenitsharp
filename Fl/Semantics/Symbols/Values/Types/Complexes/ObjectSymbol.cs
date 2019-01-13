// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Symbols
{
    public class ObjectSymbol : ComplexSymbol
    {
        public ObjectSymbol(string name, ISymbolContainer parent)
            : base(name, BuiltinType.Object, parent)
        {
        }

        public IBoundSymbol CreateProperty(string name, ITypeSymbol type, Access access, Storage storage)
        {
            var symbol = new BoundSymbol(name, type, access, storage, this);

            this.Insert(name, symbol);
            this.Properties[name] = symbol;

            return symbol;
        }

        public IBoundSymbol CreateFunction(string name, ITypeSymbol type, Access access)
        {
            var symbol = new BoundSymbol(name, type, access, Storage.Constant, this);

            this.Insert(name, symbol);
            this.Functions[name] = symbol;

            return symbol;
        }

        public override string ToValueString()
        {
            var members = new List<string>();

            var symbols = this.Symbols.Where(kvp => kvp.Value is IBoundSymbol).Select(kvp => kvp.Value as IBoundSymbol).ToList();

            foreach (var value in symbols)
                members.Add(value.ToValueString());

            if (members.Any())
                return $"{{ {string.Join(", ", members)} }}";

            return "{}";
        }

        public override string ToSafeString(params (ITypeSymbol type, string safestr)[] safeTypes)
        {
            return this.ToValueString();
        }
    }
}
