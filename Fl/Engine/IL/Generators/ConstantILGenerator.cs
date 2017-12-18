// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.IL.Generators
{
    class ConstantILGenerator : INodeVisitor<AstILGenerator, AstConstantNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstConstantNode constdec)
        {
            // Get the variable type
            ObjectType t = ObjectType.GetFromTokenType(constdec.Type.Type);

            foreach (var declaration in constdec.Constants)
            {
                // Get the identifier name
                var identifierToken = declaration.Item1;
                // Get the right-hand side operand
                var operand = declaration.Item2.Exec(generator);

                // const <identifier> = <operand>
                var symbol = new SymbolOperand(identifierToken.Value.ToString());
                generator.Emmit(new ConstInstruction(symbol, t?.ClassName ?? operand?.TypeName, operand));
            }
            return null;
        }
    }
}
