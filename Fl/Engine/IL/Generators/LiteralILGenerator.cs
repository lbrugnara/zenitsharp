// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;
using System;

namespace Fl.Engine.IL.Generators
{
    class LiteralILGenerator : INodeVisitor<AstILGenerator, AstLiteralNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstLiteralNode literal)
        {
            // Get the literal's object type
            ObjectType type = ObjectType.GetFromTokenType(literal.Literal.Type);

            // Take the raw value
            var value = literal.Literal.Value;

            // Return an immediate operand
            return new ImmediateOperand(type.ClassName, value);
        }
    }
}
