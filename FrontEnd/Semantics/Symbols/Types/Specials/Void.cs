// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Symbols.Types.Specials
{
    public class Void : ISpecialType
    {
        public string Name => "void";

        public IContainer Parent => null;

        public BuiltinType BuiltinType => BuiltinType.Void;        

        public Void()
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
