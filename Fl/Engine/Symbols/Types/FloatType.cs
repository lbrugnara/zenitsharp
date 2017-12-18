// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class FloatType : NumericType
    {
        private static FloatType _Instance;

        private FloatType() { }

        public static FloatType Value => _Instance != null ? _Instance : (_Instance = new FloatType());

        public override string Name => "float";

        public override string ClassName => "float";

        public override object RawDefaultValue()
        {
            return 0.0f;
        }

        public override FlObject DefaultValue()
        {
            return new FlFloat(0.0f);
        }

        public override FlObject NewValue(object o)
        {
            return new FlFloat(float.Parse(o.ToString()));
        }
    }
}
