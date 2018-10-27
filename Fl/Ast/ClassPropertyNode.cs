// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class ClassPropertyNode : Node
    {
        public Token Name { get; }
        public SymbolInformation SymbolInfo { get; }
        public Node Definition { get; }

        public ClassPropertyNode(Token name, SymbolInformation symbolModifiers, Node definition)
        {
            this.Name = name;
            this.SymbolInfo = symbolModifiers;
            this.Definition = definition;
        }
    }
}
