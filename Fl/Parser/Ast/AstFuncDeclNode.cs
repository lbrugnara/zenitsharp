// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;

namespace Fl.Parser.Ast
{
    public class AstParametersNode : AstNode
    {
        public List<Token> Parameters { get; }

        public AstParametersNode(List<Token> parameters)
        {
            Parameters = parameters;
        }
    }

    public class AstFuncDeclNode : AstNode
    {
        public Token Identifier { get; }
        public AstParametersNode Parameters { get; }
        public List<AstNode> Body { get; }

        public AstFuncDeclNode(Token name, AstParametersNode parameters, List<AstNode> body)
        {
            Identifier = name;
            Parameters = parameters;
            Body = body;
        }
    }
}
