// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Fl.Ast
{
    public class AstWalkerException : Exception
    {
        public AstWalkerException(string msg)
            : base(msg)
        {
        }
    }
}
