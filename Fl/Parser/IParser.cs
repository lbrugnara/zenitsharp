// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using System.Collections.Generic;

namespace Fl.Parser
{
    public interface IParser
    {
        AstNode Parse(List<Token> tokens);
    }
}
