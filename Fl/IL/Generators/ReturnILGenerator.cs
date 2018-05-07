// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.Engine.Symbols.Exceptions;
using Fl.Parser.Ast;

namespace Fl.IL.Generators
{
    class ReturnILGenerator : INodeVisitor<ILGenerator, AstReturnNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstReturnNode rnode)
        {
            if (!generator.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            Operand expr = rnode.ReturnTuple?.Visit(generator);
            generator.Emmit(new ReturnInstruction(expr));
            return null;
        }
    }
}
