// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;
using Zenit.Ast;

namespace Zenit.IL.Generators
{
    class ConstantILGenerator : INodeVisitor<ILGenerator, ConstantNode, Operand>
    {
        public Operand Visit(ILGenerator generator, ConstantNode constdec)
        {
            foreach (var definition in constdec.Definitions)
            {
                // Get the identifier name
                var constantName = definition.Left.Value;

                // Get the constant's type
                /*var type = OperandType.FromToken(constdec.Type);

                // Get the right-hand side operand (a must for a constant)
                var rhs = declaration.Item2.Visit(generator);

                // const <identifier> = <operand>
                var symbol = generator.SymbolTable.NewSymbol(constantName, type);

                generator.Emmit(new ConstInstruction(symbol, type, rhs));*/
            }
            return null;
        }
    }
}
