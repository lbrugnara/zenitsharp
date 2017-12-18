// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Engine.Symbols.Types
{
    public class CharType : ObjectType
    {
        private static CharType _Instance;

        private CharType() { }

        public static CharType Value => _Instance != null ? _Instance : (_Instance = new CharType());

        public override string Name => "char";

        public override string ClassName => "char";

        public override object RawDefaultValue()
        {
            return '\0';
        }

        public override FlObject DefaultValue()
        {
            return new FlChar('\0');
        }

        public override FlObject NewValue(object o)
        {
            return new FlChar(char.Parse(o.ToString()));
        }
    }
}
