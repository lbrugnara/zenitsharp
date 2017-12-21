using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols.Exceptions
{
    public class UnsupportedOperandException : Exception
    {
        public UnsupportedOperandException(string msg)
            : base (msg)
        {

        }
    }
}
