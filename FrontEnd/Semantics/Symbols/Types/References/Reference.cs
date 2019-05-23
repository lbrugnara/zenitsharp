// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Symbols.Variables;
using Zenit.Semantics.Types;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zenit.Semantics.Symbols.Types.References
{
    public abstract class Reference : Container, IReference
    {
        public BuiltinType BuiltinType { get; }

        protected Reference(string name, BuiltinType type, IContainer parent)
            : base(name, parent)
        {
            this.BuiltinType = type;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var objectType = obj as Reference;

            if (objectType == null)
                return false;

            var objectSymbols = objectType.Symbols.Values.Where(s => s is IVariable).ToList();

            foreach (var member in objectSymbols)
            {
                if (!this.Symbols.ContainsKey(member.Name) || this.Symbols[member.Name] != member)
                    return false;
            }

            return true;
        }

        public abstract string ToSafeString(params (IType type, string safestr)[] safeTypes);

        public override int GetHashCode()
        {
            var hashCode = 576743166;
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, ISymbol>>.Default.GetHashCode(Symbols);
            hashCode = hashCode * -1521134295 + BuiltinType.GetHashCode();
            return hashCode;
        }
    }
}
