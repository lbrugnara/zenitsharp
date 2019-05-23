// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Symbols.Variables;
using Zenit.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Zenit.Semantics.Symbols.Types.References
{
    public class Object : Reference
    {
        public Object(string name, IContainer parent)
            : base(name, BuiltinType.Object, parent)
        {
            // this.Insert<IVariable>(BuiltinSymbol.This.GetName(), new Variable(BuiltinSymbol.This.GetName(), this, Access.Private, Storage.Immutable, this));
        }

        public IVariable CreateProperty(string name, IType type, Access access, Storage storage)
        {
            var symbol = new Variable(name, type, access, storage, this);

            this.Insert(name, symbol);
            //this.Properties[name] = symbol;

            return symbol;
        }

        public IVariable CreateFunction(string name, IType type, Access access)
        {
            var symbol = new Variable(name, type, access, Storage.Constant, this);

            this.Insert(name, symbol);
            //this.Functions[name] = symbol;

            return symbol;
        }

        public override string ToValueString()
        {
            var members = new List<string>();

            var symbols = this.Symbols.Where(kvp => kvp.Value is IVariable).Select(kvp => kvp.Value as IVariable).ToList();

            foreach (var value in symbols)
            {
                if (value.Name == BuiltinSymbol.This.GetName())
                    continue;

                members.Add(value.ToValueString());
            }

            if (members.Any())
                return $"{{ {string.Join(", ", members)} }}";

            return "{}";
        }

        public override string ToSafeString(params (IType type, string safestr)[] safeTypes)
        {
            return this.ToValueString();
        }

        public override string ToString()
        {
            return this.ToValueString();
        }
    }
}
