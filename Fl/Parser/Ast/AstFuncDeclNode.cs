// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Parser.Ast
{
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
