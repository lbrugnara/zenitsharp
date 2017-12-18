// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class DecimalType : NumericType
    {
        private static DecimalType _Instance;

        private DecimalType() { }

        public static DecimalType Value => _Instance != null ? _Instance : (_Instance = new DecimalType());

        public override string Name => "decimal";

        public override string ClassName => "decimal";

        public override object RawDefaultValue()
        {
            return 0.0M;
        }

        public override FlObject DefaultValue()
        {
            return new FlDecimal(0.0M);
        }

        public override FlObject NewValue(object o)
        {
            return new FlDecimal(decimal.Parse(o.ToString()));
        }
    }
}
