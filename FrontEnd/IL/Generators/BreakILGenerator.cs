// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions;
using Zenit.IL.Instructions.Operands;
using Zenit.Ast;

namespace Zenit.IL.Generators
{
    class BreakILGenerator : INodeVisitor<ILGenerator, BreakNode, Operand>
    {
        public Operand Visit(ILGenerator generator, BreakNode wnode)
        {
            var loopBlock = generator.SymbolTable.GetLoopBlock();
            generator.Emmit(new GotoInstruction(loopBlock.ExitPoint));
            return null;
        }
    }
}
