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
    class DeclarationILGenerator : INodeVisitor<ILGenerator, AstDeclarationNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
            {
                statement.Exec(generator);
            }
            return null;
        }
    }
}
