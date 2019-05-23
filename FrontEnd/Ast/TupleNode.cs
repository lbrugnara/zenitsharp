// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Zenit.Ast
{
    public class TupleMember
    {
        public string Name { get; }
        public Node Expression { get; }

        public TupleMember(Node expression)
            : this(null, expression)
        {
        }

        public TupleMember(string name, Node expression)
        {
            this.Name = name;
            this.Expression = expression;
        }
    }

    public class TupleNode : Node
    {
        public List<TupleMember> Items { get; }

        public TupleNode(List<TupleMember> init)
        {
            this.Items = init;
        }
    }
}
