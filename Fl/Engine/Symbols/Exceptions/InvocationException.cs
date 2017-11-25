// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols.Exceptions
{
    public class InvocationException : Exception
    {
        public InvocationException(string msg) : base(msg)
        {
        }
    }
}
