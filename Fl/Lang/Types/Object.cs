// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;

namespace Fl.Lang.Types
{
    public class Object : Type
    {
        public static Object Instance { get; } = new Object();

        private Object()
            : base("object")
        {
        }
    }
}
