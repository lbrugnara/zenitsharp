using System;

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
