// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class ForNode : Node
    {
        public Token Keyword { get; }
        public Node Init { get; }
        public Node Condition { get; }
        public Node Increment { get; }
        public Node Body { get; }

        public ForNode(Token keyword, Node init, Node condition, Node increment, Node body)
        {
            this.Keyword = keyword;
            this.Init = init;
            this.Condition = condition;
            this.Increment = increment;
            this.Body = body;
        }
    }
}
