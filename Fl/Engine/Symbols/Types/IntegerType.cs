// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class IntegerType : NumericType
    {
        private static IntegerType _Instance;

        private IntegerType() { }

        public static IntegerType Value => _Instance != null ? _Instance : (_Instance = new IntegerType());

        public override string Name => "int";

        public override string ClassName => "int";
    }
}
