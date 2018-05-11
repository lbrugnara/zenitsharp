// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;
using Fl.Ast;

namespace Fl.IL.Generators
{
    class TupleILGenerator : INodeVisitor<ILGenerator, AstTupleNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstTupleNode node)
        {
            if (node.Items.Count > 0)
            {
                foreach (var item in node.Items)
                {
                    return item.Visit(generator);
                }
            }

            return null;
        }
    }
}
