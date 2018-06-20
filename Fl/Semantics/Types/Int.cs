// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Types
{
    public class Int : Primitive
    {
        public static Int Instance { get; } = new Int();

        private Int()
            : base("int")
        {
        }
    }
}
