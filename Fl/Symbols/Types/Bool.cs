// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Symbols.Types
{
    public class Bool : Primitive
    {
        public static Bool Instance { get; } = new Bool();

        private Bool()
            : base ("bool")
        {
        }
    }
}
