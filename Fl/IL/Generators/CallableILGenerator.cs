// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.Engine.Symbols.Types;
using Fl.Ast;
using System.Collections.Generic;
using System.Linq;

namespace Fl.IL.Generators
{
    public class CallableILGenerator : INodeVisitor<ILGenerator, AstCallableNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstCallableNode node)
        {
            Operand target = node.Callable.Visit(generator);

            // Generate the "param" instructions
            List<ParamInstruction> parameters = node.Arguments.Expressions.Select(a => new ParamInstruction(a.Visit(generator))).ToList();
            parameters.Reverse();

            parameters.ForEach(p => generator.Emmit(p));

            // Generate the call instruction
            generator.Emmit(new CallInstruction(target, parameters.Count));

            SymbolOperand dest = generator.SymbolTable.NewTempSymbol(OperandType.Null);
            generator.Emmit(new VarInstruction(dest, OperandType.Null));
            return dest;
        }
    }
}
