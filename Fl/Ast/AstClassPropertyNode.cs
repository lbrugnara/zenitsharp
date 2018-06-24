// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstClassPropertyNode : AstNode
    {
        public Token Name { get; }
        public SymbolInformation SymbolInfo { get; }
        public AstNode Definition { get; }

        public AstClassPropertyNode(Token name, SymbolInformation symbolModifiers, AstNode definition)
        {
            this.Name = name;
            this.SymbolInfo = symbolModifiers;
            this.Definition = definition;
        }
    }
}
