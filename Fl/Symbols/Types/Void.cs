// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Symbols.Types
{
    public class Void : Primitive
    {
        public static Void Instance { get; } = new Void();

        private Void()
            : base("void")
        {
        }
    }
}
