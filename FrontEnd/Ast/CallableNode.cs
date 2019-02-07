// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Syntax;

namespace Zenit.Ast
{
    public class CallableNode : Node
    {
        public Node Target { get; }
        public ExpressionListNode Arguments { get; }
        public Token New { get; }

        public CallableNode(Node callable, ExpressionListNode args, Token newt = null)
        {
            this.Target = callable;
            this.Arguments = args;
            this.New = newt;
        }
    }
}
