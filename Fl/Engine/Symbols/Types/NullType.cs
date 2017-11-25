// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class NullType : ObjectType
    {
        private static NullType _Instance;

        private NullType() { }

        public static NullType Value => _Instance != null ? _Instance : (_Instance = new NullType());

        public override string Name => "null";

        public override string ClassName => "null";
    }
}
