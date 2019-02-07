// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions;
using Zenit.IL.Instructions.Operands;
using Zenit.Ast;

namespace Zenit.IL.Generators
{
    class ContinueILGenerator : INodeVisitor<ILGenerator, ContinueNode, Operand>
    {
        public Operand Visit(ILGenerator generator, ContinueNode cnode)
        {
            var loopBlock = generator.SymbolTable.GetLoopBlock();
            generator.Emmit(new GotoInstruction(loopBlock.EntryPoint));
            return null;
        }
    }
}
