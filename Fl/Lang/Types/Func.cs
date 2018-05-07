// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Lang.Types
{
    public class Func : Type
    {
        public static Func Instance { get; } = new Func();

        private Func()
            : base("func")
        {
        }
    }
}
