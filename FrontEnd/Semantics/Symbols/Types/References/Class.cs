// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Symbols.Variables;
using Zenit.Semantics.Types;
using System.Collections.Generic;

namespace Zenit.Semantics.Symbols.Types.References
{
    public class Class : Reference
    {
        // TODO: Update constants to be a dictionary <string, IValueSymbol>
        protected List<string> Constants { get; set; }

        // TODO: Update methods to be a dictionary <string, IValueSymbol>
        protected List<string> Methods { get; set; }

        public Class(string name, IContainer parent = null)
            : base(name, BuiltinType.Class, parent)
        {
            this.Constants = new List<string>();
            this.Methods = new List<string>();
        }

        public IVariable CreateProperty(string name, IType type, Access access, Storage storage)
        {
            var symbol = new Variable(name, type, access, storage, this);

            this.Insert(name, symbol);
            //this.Properties[name] = symbol;

            return symbol;
        }

        public IVariable CreateConstant(string name, IType type, Access access)
        {
            var symbol = new Variable(name, type, access, Storage.Constant, this);

            this.Insert(name, symbol);
            this.Constants.Add(name);

            return symbol;
        }

        public IVariable CreateMethod(string name, IType type, Access access)
        {
            var symbol = new Variable(name, type, access, Storage.Constant, this);

            this.Insert(name, symbol);
            this.Methods.Add(name);

            return symbol;
        }

        public override string ToValueString()
        {
            return "class (FIXME)";
        }

        public override string ToSafeString(params (IType type, string safestr)[] safeTypes)
        {
            return ToValueString();
        }
    }
}
