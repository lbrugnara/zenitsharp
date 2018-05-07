// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;

namespace Fl.Lang.Types
{
    public class Error : Type
    {
        public static Error Instance { get; } = new Error();

        private Error()
            : base("Error")
        {
        }
    }
}
