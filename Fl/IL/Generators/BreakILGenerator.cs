// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.Parser.Ast;

namespace Fl.IL.Generators
{
    class BreakILGenerator : INodeVisitor<ILGenerator, AstBreakNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstBreakNode wnode)
        {
            var loopBlock = generator.SymbolTable.GetLoopBlock();
            generator.Emmit(new GotoInstruction(loopBlock.ExitPoint));
            return null;
        }
    }
}
