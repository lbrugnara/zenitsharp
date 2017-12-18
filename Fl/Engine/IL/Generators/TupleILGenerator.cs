// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System.Linq;

namespace Fl.Engine.IL.Generators
{
    class TupleILGenerator : INodeVisitor<AstILGenerator, AstTupleNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstTupleNode node)
        {
            return null;
        }
    }
}
