// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser;

namespace Fl.Ast
{
    public class AstWhileNode : AstNode
    {
        public Token Keyword { get; }
        public AstNode Condition { get; }
        public AstNode Body { get; }

        public AstWhileNode(Token token, AstNode condition, AstNode body)
        {
            this.Keyword = token;
            this.Condition = condition;
            this.Body = body;
        }
    }
}
