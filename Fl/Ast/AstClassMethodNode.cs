// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Syntax;

namespace Fl.Ast
{
    public class AstClassMethodNode : AstNode
    {
        public Token Identifier { get; }
        public Token AccessModifier { get; }
        public AstParametersNode Parameters { get; }
        public List<AstNode> Body { get; }
        public bool IsLambda { get; }

        public AstClassMethodNode(Token name, Token accessModifier, AstParametersNode parameters, List<AstNode> body, bool isLambda)
        {
            this.Identifier = name;
            this.AccessModifier = accessModifier;
            this.Parameters = parameters;
            this.Body = body;
            this.IsLambda = isLambda;
        }

        public string Name => this.Identifier.Value.ToString();
    }
}
