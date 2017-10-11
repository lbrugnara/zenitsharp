// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Parser.Ast
{
    public interface IAstWalker<T> where T : class
    {
        T Process(AstNode node);
    }
}
