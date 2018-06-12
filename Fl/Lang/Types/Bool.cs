// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Lang.Types
{
    public class Bool : PrimitiveType
    {
        public static Bool Instance { get; } = new Bool();

        private Bool()
            : base ("bool")
        {
        }
    }
}
