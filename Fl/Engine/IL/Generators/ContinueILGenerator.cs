// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class ContinueILGenerator : INodeVisitor<ILGenerator, AstContinueNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstContinueNode cnode)
        {
            var loopBlock = generator.SymbolTable.GetLoopBlock();
            generator.Emmit(new GotoInstruction(loopBlock.EntryPoint));
            return null;
        }
    }
}
