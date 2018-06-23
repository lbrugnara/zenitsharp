// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstClassPropertyNode : AstNode
    {
        public Token Name { get; }
        public Token AccessModifier { get; }
        public Token StorageType { get; }
        public AstTypeNode Type { get; }
        public AstNode Definition { get; }

        public AstClassPropertyNode(Token name, Token accessModifier, Token storageModifier, AstTypeNode type, AstNode definition)
        {
            this.Name = name;
            this.AccessModifier = accessModifier;
            this.StorageType = storageModifier;
            this.Type = type;
            this.Definition = definition;
        }
    }
}
