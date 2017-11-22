// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class BoolType : ObjectType
    {
        private static BoolType _Instance;

        private BoolType() { }

        public static BoolType Value => _Instance != null ? _Instance : (_Instance = new BoolType());

        public override string ToString()
        {
            return "bool";
        }
    }
}
