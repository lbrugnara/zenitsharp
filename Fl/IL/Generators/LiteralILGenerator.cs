// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;

namespace Fl.IL.Generators
{
    class LiteralILGenerator : INodeVisitor<ILGenerator, AstLiteralNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstLiteralNode literal)
        {
            // Take the raw value
            var value = literal.Literal.Value;

            // Return an immediate operand
            return new ImmediateOperand(OperandType.FromToken(literal.Literal), value);
        }
    }
}
