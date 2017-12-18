// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class NullType : ObjectType
    {
        private static NullType _Instance;

        private NullType() { }

        public static NullType Value => _Instance != null ? _Instance : (_Instance = new NullType());

        public override string Name => "null";

        public override string ClassName => "null";

        public override object RawDefaultValue()
        {
            return null;
        }

        public override FlObject DefaultValue()
        {
            return FlNull.Value;
        }

        public override FlObject NewValue(object o)
        {
            return FlNull.Value;
        }
    }
}
