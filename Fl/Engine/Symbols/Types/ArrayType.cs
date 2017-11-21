// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class ArrayType : ObjectType
    {
        private static ArrayType _Instance;

        private ArrayType() { }

        public static ArrayType Value => _Instance != null ? _Instance : (_Instance = new ArrayType());

        public override string ToString()
        {
            return "array";
        }
    }
}
