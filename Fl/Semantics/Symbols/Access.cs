// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Symbols
{
    public enum Access
    {
        Public,
        Protected,
        Private
    }

    public static class AccessExtensions
    {
        public static string ToKeyword(this Access s)
        {
            switch (s)
            {
                case Access.Public:
                    return "public";

                case Access.Protected:
                    return "protected";

                case Access.Private:
                    return "private";

                default:
                    throw new System.Exception($"Unhandled access type {s}");
            }
        }
    }
}
