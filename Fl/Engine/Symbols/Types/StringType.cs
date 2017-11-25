// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class StringType : ObjectType
    {
        private static StringType _Instance;

        private StringType() { }

        public static StringType Value => _Instance != null ? _Instance : (_Instance = new StringType());

        public override string Name => "string";

        public override string ClassName => "string";
    }
}
