// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class ContinueILGenerator : INodeVisitor<AstILGenerator, AstContinueNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstContinueNode cnode)
        {
            return null;
        }
    }
}
