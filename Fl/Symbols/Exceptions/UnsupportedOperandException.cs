using System;

namespace Fl.Symbols.Exceptions
{
    public class UnsupportedOperandException : Exception
    {
        public UnsupportedOperandException(string msg)
            : base (msg)
        {

        }
    }
}
