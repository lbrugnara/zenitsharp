// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class DoubleType : NumericType
    {
        private static DoubleType _Instance;

        private DoubleType() { }

        public static DoubleType Value => _Instance != null ? _Instance : (_Instance = new DoubleType());

        public override string ToString()
        {
            return "double";
        }
    }
}
