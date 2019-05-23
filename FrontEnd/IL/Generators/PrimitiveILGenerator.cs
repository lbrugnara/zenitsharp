// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;
using Zenit.Engine.Symbols.Types;
using Zenit.Ast;

namespace Zenit.IL.Generators
{
    class PrimitiveILGenerator : INodeVisitor<ILGenerator, PrimitiveNode, Operand>
    {
        public Operand Visit(ILGenerator generator, PrimitiveNode literal)
        {
            // Take the raw value
            var value = literal.Literal.Value;

            // Return an immediate operand
            return new ImmediateOperand(OperandType.FromToken(literal.Literal), value);
        }
    }
}
