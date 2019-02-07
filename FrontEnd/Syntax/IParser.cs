// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using System.Collections.Generic;

namespace Zenit.Syntax
{
    public interface IParser
    {
        Node Parse(List<Token> tokens);
    }
}
