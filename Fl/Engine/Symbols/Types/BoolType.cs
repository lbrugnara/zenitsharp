// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class BoolType : ObjectType
    {
        private static BoolType _Instance;

        private BoolType() { }

        public static BoolType Value => _Instance != null ? _Instance : (_Instance = new BoolType());

        public override string Name => "bool";

        public override string ClassName => "bool";

        public override object RawDefaultValue()
        {
            return false;
        }

        public override FlObject DefaultValue()
        {
            return new FlBool(false);
        }

        public override FlObject NewValue(object o)
        {
            return new FlBool(bool.Parse(o.ToString()));
        }
    }
}
