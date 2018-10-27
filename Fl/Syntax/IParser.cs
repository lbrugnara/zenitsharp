// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using System.Collections.Generic;

namespace Fl.Syntax
{
    public interface IParser
    {
        Node Parse(List<Token> tokens);
    }
}
