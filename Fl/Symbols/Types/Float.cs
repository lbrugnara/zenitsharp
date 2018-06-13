// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Symbols.Types
{
    public class Float : Primitive
    {
        public static Float Instance { get; } = new Float();

        private Float()
            : base("float")
        {
        }
    }
}
