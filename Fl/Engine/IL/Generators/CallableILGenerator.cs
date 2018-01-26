// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Parser.Ast;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.IL.Generators
{
    public class CallableILGenerator : INodeVisitor<ILGenerator, AstCallableNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstCallableNode node)
        {
            Operand target = node.Callable.Exec(generator);

            // Declare the var to keep the result (if needed)
            var dest = generator.SymbolTable.NewTempSymbol();
            generator.Emmit(new VarInstruction(dest, null, null));

            // Generate the "param" instructions
            List<ParamInstruction> parameters = node.Arguments.Expressions.Select(a => new ParamInstruction(a.Exec(generator))).ToList();
            parameters.Reverse();

            parameters.ForEach(p => generator.Emmit(p));

            // Generate the call instruction
            generator.Emmit(new CallInstruction(dest, target, parameters.Count));
            return dest;
        }
    }
}
