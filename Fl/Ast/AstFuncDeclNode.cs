// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Syntax;

namespace Fl.Ast
{
    public class AstFuncDeclNode : AstNode
    {
        private Token identifier;

        public AstFuncDeclNode(Token name, AstParametersNode parameters, List<AstNode> body, bool isAnonymous, bool isLambda)
        {
            this.identifier = name;
            this.Parameters = parameters;
            this.Body = body;
            this.IsAnonymous = isAnonymous;
            this.IsLambda = isLambda;
        }

        public AstParametersNode Parameters { get; }
        public List<AstNode> Body { get; }
        public bool IsAnonymous { get; }
        public bool IsLambda { get; }
        public string Name => this.IsAnonymous ? $"@func{this.GetHashCode()}" : this.identifier.Value.ToString();
    }
}
