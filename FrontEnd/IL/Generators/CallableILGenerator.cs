// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions;
using Zenit.IL.Instructions.Operands;
using Zenit.Engine.Symbols.Types;
using Zenit.Ast;
using System.Collections.Generic;
using System.Linq;

namespace Zenit.IL.Generators
{
    public class CallableILGenerator : INodeVisitor<ILGenerator, CallableNode, Operand>
    {
        public Operand Visit(ILGenerator generator, CallableNode node)
        {
            Operand target = node.Target.Visit(generator);

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
