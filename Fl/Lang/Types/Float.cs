// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Lang.Types
{
    public class Float : Type
    {
        public static Float Instance { get; } = new Float();

        private Float()
            : base("float")
        {
        }
    }
}
