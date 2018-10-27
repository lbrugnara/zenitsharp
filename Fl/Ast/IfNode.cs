// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;

namespace Fl.Ast
{
    public class IfNode : Node
    {
        public Token Keyword { get; }
        public Node Condition { get; }
        public Node Then { get; }
        public Node Else { get; }

        public IfNode(Token keyword, Node condition, Node thenbranch, Node elsebranch)
        {
            this.Keyword = keyword;
            this.Condition = condition;
            this.Then = thenbranch;
            this.Else = elsebranch;
        }
    }
}
