// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class DecimalType : NumericType
    {
        private static DecimalType _Instance;

        private DecimalType() { }

        public static DecimalType Value => _Instance != null ? _Instance : (_Instance = new DecimalType());

        public override string Name => "decimal";

        public override string ClassName => "decimal";
    }
}
