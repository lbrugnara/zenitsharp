// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.IL.Generators
{
    class AccessorILGenerator : INodeVisitor<AstILGenerator, AstAccessorNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstAccessorNode accessor)
        {
            var member = new SymbolOperand(accessor.Identifier.Value.ToString());
            if (accessor.Enclosing == null)
                return member;

            Operand parent = accessor.Enclosing.Exec(generator);
            parent.AddMember(member);
            return parent;
        }
    }
}
