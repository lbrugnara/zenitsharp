// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Parser;

namespace Fl.Ast
{
    public class AstFuncDeclNode : AstNode
    {
        public Token Identifier { get; }
        public AstParametersNode Parameters { get; }
        public List<AstNode> Body { get; }
        public bool IsAnonymous { get; }
        public bool IsLambda { get; }

        public AstFuncDeclNode(Token name, AstParametersNode parameters, List<AstNode> body, bool isAnonymous, bool isLambda)
        {
            this.Identifier = name;
            this.Parameters = parameters;
            this.Body = body;
            this.IsAnonymous = isAnonymous;
            this.IsLambda = isLambda;
        }
    }
}
