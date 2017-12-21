// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.IL.Generators
{
    public class CallableILGenerator : INodeVisitor<AstILGenerator, AstCallableNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstCallableNode node)
        {
            Operand op = node.Callable.Exec(generator);

            // Declare the var to keep the result (if needed)
            var dest = generator.GenerateTemporalName();
            generator.Emmit(new VarInstruction(dest, null, null));

            // Generate the "param" instructions
            List<ParamInstruction> parameters = node.Arguments.Expressions.Select(a => new ParamInstruction(a.Exec(generator))).ToList();
            parameters.Reverse();

            parameters.ForEach(p => generator.Emmit(p));

            // Generate the call instruction
            generator.Emmit(new CallInstruction(dest, op, parameters.Count));
            return dest;
        }
    }
}
