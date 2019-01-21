// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols.Types.Specials
{
    public class VoidSymbol : ISpecialTypeSymbol
    {
        public string Name => "void";

        public IContainer Parent => null;

        public BuiltinType BuiltinType => BuiltinType.Void;        

        public VoidSymbol()
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
