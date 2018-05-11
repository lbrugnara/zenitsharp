// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions.Operands;
using Fl.Ast;

namespace Fl.IL.Generators
{
    class DeclarationILGenerator : INodeVisitor<ILGenerator, AstDeclarationNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstDeclarationNode decls)
        {
            foreach (AstNode statement in decls.Statements)
            {
                statement.Visit(generator);
            }
            return null;
        }
    }
}
