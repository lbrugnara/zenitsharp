// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Zenit.Semantics.Exceptions
{
    public class InvocationException : Exception
    {
        public InvocationException(string msg) : base(msg)
        {
        }
    }
}
