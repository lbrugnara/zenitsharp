// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Lang.Types
{
    public class Float : PrimitiveType
    {
        public static Float Instance { get; } = new Float();

        private Float()
            : base("float")
        {
        }
    }
}
