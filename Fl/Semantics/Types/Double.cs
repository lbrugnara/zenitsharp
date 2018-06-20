// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Types
{
    public class Double : Primitive
    {
        public static Double Instance { get; } = new Double();

        private Double()
            : base("double")
        {
        }
    }
}
