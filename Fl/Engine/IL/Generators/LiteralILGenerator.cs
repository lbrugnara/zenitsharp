// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class LiteralILGenerator : INodeVisitor<ILGenerator, AstLiteralNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstLiteralNode literal)
        {
            // Get the literal's object type
            TypeResolver typeresolver = TypeResolver.GetTypeResolverFromToken(literal.Literal);

            // Take the raw value
            var value = literal.Literal.Value;

            // Return an immediate operand
            return new ImmediateOperand(typeresolver, value);
        }
    }
}
