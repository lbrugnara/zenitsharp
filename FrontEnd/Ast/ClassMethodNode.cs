// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Zenit.Syntax;

namespace Zenit.Ast
{
    public class ClassMethodNode : Node
    {
        public Token Identifier { get; }
        public SymbolInformation SymbolInfo { get; }
        public List<ParameterNode> Parameters { get; }
        public List<Node> Body { get; }
        public bool IsLambda { get; }

        public ClassMethodNode(Token name, SymbolInformation modifiers, List<ParameterNode> parameters, List<Node> body, bool isLambda)
        {
            this.Identifier = name;
            this.SymbolInfo = modifiers;
            this.Parameters = parameters;
            this.Body = body;
            this.IsLambda = isLambda;
        }

        public string Name => this.Identifier.Value;
    }
}
