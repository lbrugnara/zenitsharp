// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class AstAccessorNode : AstNode
    {
        public readonly Token Identifier;
        public readonly AstNode Enclosing;
        public readonly bool IsCall;

        public AstAccessorNode(Token identifier, AstNode enclosing, bool isCall = false)
        {
            this.Identifier = identifier;
            this.Enclosing = enclosing;
            this.IsCall = isCall;
        }
    }
}
