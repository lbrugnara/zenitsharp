// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Lang.Types
{
    public class Null : Type
    {
        public static Null Instance { get; } = new Null();

        private Null()
            : base("null")
        {
        }
    }
}
