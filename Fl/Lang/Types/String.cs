// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Lang.Types
{
    public class String : PrimitiveType
    {
        public static String Instance { get; } = new String();

        private String()
            : base("string")
        {
        }
    }
}
