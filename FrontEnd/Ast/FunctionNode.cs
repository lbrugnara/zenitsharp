// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Zenit.Syntax;

namespace Zenit.Ast
{
    public class FunctionNode : Node
    {
        private Token identifier;

        public FunctionNode(Token name, List<ParameterNode> parameters, List<Node> body, bool isAnonymous, bool isLambda)
        {
            this.identifier = name;
            this.Parameters = parameters;
            this.Body = body;
            this.IsAnonymous = isAnonymous;
            this.IsLambda = isLambda;
        }

        public List<ParameterNode> Parameters { get; }
        public List<Node> Body { get; }
        public bool IsAnonymous { get; }
        public bool IsLambda { get; }
        public string Name => this.IsAnonymous ? $"@func{this.GetHashCode()}" : this.identifier.Value;
    }
}
