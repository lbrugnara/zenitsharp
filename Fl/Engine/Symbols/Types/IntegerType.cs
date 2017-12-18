// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class IntegerType : NumericType
    {
        private static IntegerType _Instance;

        private IntegerType() { }

        public static IntegerType Value => _Instance != null ? _Instance : (_Instance = new IntegerType());

        public override string Name => "int";

        public override string ClassName => "int";

        public override object RawDefaultValue()
        {
            return 0;
        }

        public override FlObject DefaultValue()
        {
            return new FlInteger(0);
        }

        public override FlObject NewValue(object o)
        {
            return new FlInteger(int.Parse(o.ToString()));
        }
    }
}
