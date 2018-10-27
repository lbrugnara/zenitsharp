// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class ParameterNode
    {
        public Token Name { get; }
        public SymbolInformation SymbolInfo { get; }

        public ParameterNode(Token name, SymbolInformation symbolInfo)
        {
            this.Name = name;
            this.SymbolInfo = symbolInfo;
        }
    }
}
