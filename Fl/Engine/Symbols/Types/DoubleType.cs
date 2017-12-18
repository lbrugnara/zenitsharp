// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class DoubleType : NumericType
    {
        private static DoubleType _Instance;

        private DoubleType() { }

        public static DoubleType Value => _Instance != null ? _Instance : (_Instance = new DoubleType());

        public override string Name => "double";

        public override string ClassName => "double";

        public override object RawDefaultValue()
        {
            return 0.0;
        }

        public override FlObject DefaultValue()
        {
            return new FlDouble(0.0);
        }

        public override FlObject NewValue(object o)
        {
            return new FlDouble(double.Parse(o.ToString()));
        }
    }
}
