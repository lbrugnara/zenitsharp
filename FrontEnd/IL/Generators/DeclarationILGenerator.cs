// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions.Operands;
using Zenit.Ast;

namespace Zenit.IL.Generators
{
    class DeclarationILGenerator : INodeVisitor<ILGenerator, DeclarationNode, Operand>
    {
        public Operand Visit(ILGenerator generator, DeclarationNode decls)
        {
            foreach (Node statement in decls.Declarations)
            {
                statement.Visit(generator);
            }
            return null;
        }
    }
}
