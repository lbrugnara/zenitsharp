// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Symbols.Types
{
    public class String : Primitive
    {
        public static String Instance { get; } = new String();

        private String()
            : base("string")
        {
        }
    }
}
