// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols.Types.Specials
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
