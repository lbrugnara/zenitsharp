// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols
{
    public class FlNull : FlObject
    {
        private static FlNull _Instance;

        private FlNull()
        {
        }

        public static FlNull Value => _Instance == null ? (_Instance = new FlNull()) : _Instance;

        public override object RawValue => null;

        public override bool IsPrimitive => true;

        public override ObjectType ObjectType => NullType.Value;

        public override string ToDebugStr()
        {
            return "(null)";
        }

        public override FlObject Clone()
        {
            return new FlNull();
        }
    }
}
