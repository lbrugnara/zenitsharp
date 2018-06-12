// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Lang.Types
{
    public class Error : PrimitiveType
    {
        public static Error Instance { get; } = new Error();

        private Error()
            : base("Error")
        {
        }
    }
}
