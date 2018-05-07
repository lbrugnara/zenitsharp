// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using System.Linq;

namespace Fl.Lang.Types
{
    public class Tuple : Type
    {
        public static Tuple Instance { get; } = new Tuple();

        private Tuple()
            : base("tuple")
        {
        }
    }
}
