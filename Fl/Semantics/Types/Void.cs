// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Types
{
    public class Void : Object
    {
        public static Void Instance { get; } = new Void();

        private Void()
            : base ("void")
        {

        }
    }
}
