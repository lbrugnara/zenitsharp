// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Parser
{
    public class ParserException : Exception
    {
        public ParserException(string message)
            : base(message)
        {
        }
    }
}
