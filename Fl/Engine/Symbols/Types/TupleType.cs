// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Engine.Symbols.Types
{
    public class TupleType : ObjectType
    {
        private static TupleType _Instance;

        private TupleType() { }

        public static TupleType Value => _Instance != null ? _Instance : (_Instance = new TupleType());

        public override string Name => "tuple";

        public override string ClassName => "tuple";
    }
}
