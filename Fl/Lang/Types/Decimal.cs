// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Lang.Types
{
    public class Decimal : PrimitiveType
    {
        public static Decimal Instance { get; } = new Decimal();

        private Decimal()
            : base("decimal")
        {
        }
    }
}
