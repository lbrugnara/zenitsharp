// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Types
{
    public class Anonymous : Object
    {
        public Anonymous(string name)
            : base (BuiltinType.Anonymous, $"'{name}")
        {
        }

        public Anonymous()
            : base(BuiltinType.Anonymous, "'a")
        {
        }
    }
}
