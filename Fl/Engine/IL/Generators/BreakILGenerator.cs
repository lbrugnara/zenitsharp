// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
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
