// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Runtime.Serialization;

namespace Fl
{
    class LexerException : Exception
    {
        public LexerException(string message) : base(message)
        {
        }
    }
}