// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Zenit.Semantics.Exceptions
{
    public class CastException : Exception
    {
        public CastException(string msg) : base(msg)
        {
        }
    }
}
