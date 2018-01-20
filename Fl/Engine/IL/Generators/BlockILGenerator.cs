// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.IL.Generators
{
    class BlockILGenerator : INodeVisitor<ILGenerator, AstBlockNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstBlockNode node)
        {
            generator.EnterBlock(BlockType.Common);
            foreach (AstNode statement in node.Statements)
            {
                statement.Exec(generator);
            }
            generator.LeaveBlock();
            return null;
        }
    }
}
