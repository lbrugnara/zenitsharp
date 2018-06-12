// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Lang.Types
{
    public class Int : PrimitiveType
    {
        public static Int Instance { get; } = new Int();

        private Int()
            : base("int")
        {
        }
    }
}
