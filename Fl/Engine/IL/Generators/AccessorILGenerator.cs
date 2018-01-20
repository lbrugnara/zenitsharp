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
    class AccessorILGenerator : INodeVisitor<ILGenerator, AstAccessorNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstAccessorNode accessor)
        {
            // If it is the root member (or the only accessed one) return the symbol
            if (accessor.Enclosing == null)
                return generator.SymbolTable.GetSymbol(accessor.Identifier.Value.ToString());

            // Resolve the parent member and add current as a child
            Operand parent = accessor.Enclosing.Exec(generator);
            parent.AddMember(new SymbolOperand(accessor.Identifier.Value.ToString(), null));
            return parent;
        }
    }
}
