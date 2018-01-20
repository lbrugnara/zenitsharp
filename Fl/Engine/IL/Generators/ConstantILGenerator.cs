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
    class ConstantILGenerator : INodeVisitor<ILGenerator, AstConstantNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstConstantNode constdec)
        {
            // Get the variable type
            TypeResolver typeres = TypeResolver.GetTypeResolverFromToken(constdec.Type);

            foreach (var declaration in constdec.Constants)
            {
                // Get the identifier name
                var identifierToken = declaration.Item1;
                // Get the right-hand side operand
                var operand = declaration.Item2.Exec(generator);

                // const <identifier> = <operand>
                var symbol = generator.SymbolTable.NewSymbol(identifierToken.Value.ToString(), typeres); //new SymbolOperand(generator.SymbolTable.GetMangledName(identifierToken.Value.ToString()));
                generator.Emmit(new ConstInstruction(symbol, typeres, operand));
            }
            return null;
        }
    }
}
