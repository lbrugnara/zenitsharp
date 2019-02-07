using System;

namespace Zenit.Semantics.Exceptions
{
    public class UnsupportedOperandException : Exception
    {
        public UnsupportedOperandException(string msg)
            : base (msg)
        {

        }
    }
}
