// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Zenit.Ast
{
    public class ExpressionListNode : Node
    {
        public List<Node> Expressions { get; }

        public ExpressionListNode(List<Node> args)
        {
            this.Expressions = args;
        }

        public int Count => this.Expressions?.Count ?? 0;
    }
}
