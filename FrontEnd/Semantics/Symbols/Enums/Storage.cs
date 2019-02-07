// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Zenit.Semantics.Symbols
{
    public enum Storage
    {
        Immutable,
        Mutable,
        Constant
    }

    public static class StorageExtensions
    {
        public static string ToKeyword(this Storage s)
        {
            switch (s)
            {
                case Storage.Mutable:
                    return "mut";

                case Storage.Constant:
                    return "const";

                case Storage.Immutable:
                    return "";

                default:
                    throw new System.Exception($"Unhandled storage type {s}");
            }
        }
    }
}
