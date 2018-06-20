using System;

namespace Fl.Semantics.Exceptions
{
    public class UnsupportedOperandException : Exception
    {
        public UnsupportedOperandException(string msg)
            : base (msg)
        {

        }
    }
}
