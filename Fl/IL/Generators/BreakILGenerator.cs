// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.Ast;

namespace Fl.IL.Generators
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
