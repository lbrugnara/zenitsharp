// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

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
    class BlockILGenerator : INodeVisitor<AstILGenerator, AstBlockNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstBlockNode node)
        {
            //string scopeName = generator.GenerateCommonScopeName();
            try
            {
                //generator.SymbolTable.EnterScope(ScopeType.Common, scopeName);
                foreach (AstNode statement in node.Statements)
                {
                    statement.Exec(generator);
                }
            }
            finally
            {
                //generator.SymbolTable.LeaveScope();
            }
            // no-op
            return null;
        }
    }
}
