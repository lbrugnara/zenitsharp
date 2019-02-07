// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions;
using Zenit.IL.Instructions.Operands;
using Zenit.Ast;
using Zenit.Semantics.Exceptions;

namespace Zenit.IL.Generators
{
    class ReturnILGenerator : INodeVisitor<ILGenerator, ReturnNode, Operand>
    {
        public Operand Visit(ILGenerator generator, ReturnNode rnode)
        {
            if (!generator.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            Operand expr = rnode.Expression?.Visit(generator);
            generator.Emmit(new ReturnInstruction(expr));
            return null;
        }
    }
}
