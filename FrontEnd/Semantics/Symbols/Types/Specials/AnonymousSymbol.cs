// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Helpers;
using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Symbols.Types.Specials
{
    public class AnonymousSymbol : ISpecialTypeSymbol
    {
        public string Name { get; }

        public IContainer Parent => null;

        public BuiltinType BuiltinType => BuiltinType.Anonymous;

        public AnonymousSymbol()
        {
            this.Name = $"'{NameGenerator.Instance.Next()}";
        }

        public override bool Equals(object obj)
        {
            var objType = obj as AnonymousSymbol;

            if (objType == null)
                return false;

            return this.Name == objType.Name;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string ToValueString()
        {
            return this.Name;
        }        
    }
}
