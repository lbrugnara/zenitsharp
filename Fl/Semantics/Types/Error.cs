// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Types
{
    public class Error : Primitive
    {
        public static Error Instance { get; } = new Error();

        private Error()
            : base("Error")
        {
        }
    }
}
