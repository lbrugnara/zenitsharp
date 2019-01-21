// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class ClassConstantNode : Node
    {
        public Token Name { get; }
        public SymbolInformation SymbolInfo { get; }
        public Node Definition { get; }

        public ClassConstantNode(Token name, SymbolInformation modifiers, Node definition)
        {
            this.Name = name;
            this.SymbolInfo = modifiers;
            this.Definition = definition;
        }
    }
}
