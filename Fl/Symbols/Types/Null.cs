// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Symbols.Types
{
    public class Null : Primitive
    {
        public static Null Instance { get; } = new Null();

        private Null()
            : base("null")
        {
        }
    }
}
