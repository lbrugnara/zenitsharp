// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;
using Fl.Ast;

namespace Fl.IL.Generators
{
    class BlockILGenerator : INodeVisitor<ILGenerator, BlockNode, Operand>
    {
        public Operand Visit(ILGenerator generator, BlockNode node)
        {
            generator.EnterBlock(BlockType.Common);
            foreach (Node statement in node.Statements)
            {
                statement.Visit(generator);
            }
            generator.LeaveBlock();
            return null;
        }
    }
}
