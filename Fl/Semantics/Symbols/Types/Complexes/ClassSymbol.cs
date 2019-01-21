// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class ClassSymbol : ComplexSymbol
    {
        // TODO: Update constants to be a dictionary <string, IValueSymbol>
        protected List<string> Constants { get; set; }

        // TODO: Update methods to be a dictionary <string, IValueSymbol>
        protected List<string> Methods { get; set; }

        public ClassSymbol(string name, IContainer parent = null)
            : base(name, BuiltinType.Class, parent)
        {
            this.Constants = new List<string>();
            this.Methods = new List<string>();
        }

        public IBoundSymbol CreateProperty(string name, ITypeSymbol type, Access access, Storage storage)
        {
            var symbol = new BoundSymbol(name, type, access, storage, this);

            this.Insert(name, symbol);
            //this.Properties[name] = symbol;

            return symbol;
        }

        public IBoundSymbol CreateConstant(string name, ITypeSymbol type, Access access)
        {
            var symbol = new BoundSymbol(name, type, access, Storage.Constant, this);

            this.Insert(name, symbol);
            this.Constants.Add(name);

            return symbol;
        }

        public IBoundSymbol CreateMethod(string name, ITypeSymbol type, Access access)
        {
            var symbol = new BoundSymbol(name, type, access, Storage.Constant, this);

            this.Insert(name, symbol);
            this.Methods.Add(name);

            return symbol;
        }

        public override string ToValueString()
        {
            return "class (FIXME)";
        }

        public override string ToSafeString(params (ITypeSymbol type, string safestr)[] safeTypes)
        {
            return ToValueString();
        }
    }
}
