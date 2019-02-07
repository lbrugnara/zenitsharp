// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Symbols.Types.Specials
{
    public class NoneSymbol : ISpecialTypeSymbol
    {
        public string Name => "none";

        public IContainer Parent => null;

        public BuiltinType BuiltinType => BuiltinType.None;

        public NoneSymbol()
        {
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
