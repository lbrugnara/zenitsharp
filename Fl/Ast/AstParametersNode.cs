// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Syntax;

namespace Fl.Ast
{
    public class Parameter
    {
        public Token Name { get; }
        public SymbolInformation SymbolInfo { get; }

        public Parameter(Token name, SymbolInformation symbolInfo)
        {
            this.Name = name;
            this.SymbolInfo = symbolInfo;
        }
    }

    public class AstParametersNode : AstNode
    {
        public List<Parameter> Parameters { get; }

        public AstParametersNode(List<Parameter> parameters)
        {
            this.Parameters = parameters;
        }
    }
}
