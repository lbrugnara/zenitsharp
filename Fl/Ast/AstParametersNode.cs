// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Parser;

namespace Fl.Ast
{
    public class AstParametersNode : AstNode
    {
        public List<Token> Parameters { get; }

        public AstParametersNode(List<Token> parameters)
        {
            this.Parameters = parameters;
        }
    }
}
