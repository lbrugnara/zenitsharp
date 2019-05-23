// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;
using Zenit.Ast;

namespace Zenit.IL.Generators
{
    class TupleILGenerator : INodeVisitor<ILGenerator, TupleNode, Operand>
    {
        public Operand Visit(ILGenerator generator, TupleNode node)
        {
            if (node.Items.Count > 0)
            {
                foreach (var item in node.Items)
                {
                    return item.Expression.Visit(generator);
                }
            }

            return null;
        }
    }
}
