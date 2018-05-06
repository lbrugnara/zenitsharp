// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class ConstantILGenerator : INodeVisitor<ILGenerator, AstConstantNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstConstantNode constdec)
        {
            foreach (var declaration in constdec.Constants)
            {
                // Get the identifier name
                var constantName = declaration.Item1.Value.ToString();

                // Get the constant's type
                var type = OperandType.FromToken(constdec.Type);

                // Get the right-hand side operand (a must for a constant)
                var rhs = declaration.Item2.Exec(generator);

                // const <identifier> = <operand>
                var symbol = generator.SymbolTable.NewSymbol(constantName, type);

                generator.Emmit(new ConstInstruction(symbol, type, rhs));
            }
            return null;
        }
    }
}
