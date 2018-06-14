﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Symbols.Types
{
    public class Decimal : Primitive
    {
        public static Decimal Instance { get; } = new Decimal();

        private Decimal()
            : base("decimal")
        {
        }
    }
}