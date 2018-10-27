// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class WhileNode : Node
    {
        public Token Keyword { get; }
        public Node Condition { get; }
        public Node Body { get; }

        public WhileNode(Token token, Node condition, Node body)
        {
            this.Keyword = token;
            this.Condition = condition;
            this.Body = body;
        }
    }
}
