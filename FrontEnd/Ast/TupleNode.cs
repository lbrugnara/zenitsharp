// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Zenit.Ast
{
    public class TupleNode : Node
    {
        public List<Node> Items { get; }

        public TupleNode(List<Node> init)
        {
            this.Items = init;
        }

        public TupleNode(ExpressionListNode exprlist)
        {
            this.Items = exprlist.Expressions;
        }
    }
}
