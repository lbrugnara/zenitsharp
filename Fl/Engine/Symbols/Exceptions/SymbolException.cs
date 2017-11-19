using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols.Exceptions
{
    public class SymbolException : Exception
    {
        public SymbolException(string msg) : base(msg)
        {
        }
    }
}
