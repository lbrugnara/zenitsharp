// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Parser.Ast
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
